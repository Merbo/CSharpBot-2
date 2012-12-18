using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

using System.Text.RegularExpressions;

namespace CSharpBot
{
    class Connection
    {
        private TcpClient IRC;
        private Stream IRCStream;
        private StreamWriter Writer;
        private StreamReader Reader;

        public string Server;
        public int Port;

        public bool canRead;
        private bool isReading;

        public IRCUser UserInfo;

        public event EventHandler<IRCReadEventArgs> OnReceiveData;
        public event EventHandler<IRCHelpEventArgs> OnReceiveHelp;

        public Connection(string server, int port, IRCUser userInfo, bool ssl)
        {
            Server = server;
            Port = port;

            //Haven't implemented this yet
            bool sslCanHaveCerts = false;

            IRC = new TcpClient(server, port);
            if (!ssl)
                IRCStream = IRC.GetStream();
            else
            {
                IRCStream = new SslStream(IRC.GetStream(),
                    false,
                    new RemoteCertificateValidationCallback(ValidateServerCert),
                    sslCanHaveCerts ? new LocalCertificateSelectionCallback(ValidateLocalCert) : null);
                ((SslStream)IRCStream).AuthenticateAsClient(server);
            }
            Reader = new StreamReader(IRCStream);
            Writer = new StreamWriter(IRCStream);

            UserInfo = userInfo;

            isReading = false;
            canRead = true;
        }

        public bool ValidateServerCert(object sender, X509Certificate Cert, X509Chain Chain, SslPolicyErrors Errors)
        {
            return true;
        }

        public X509Certificate ValidateLocalCert(object sender, string name, X509CertificateCollection Collection, X509Certificate Cert, string[] names)
        {
            return null;
        }

        public void BeginRead()
        {
            if (!isReading)
            {
                isReading = true;
                new Thread(() =>
                    {
                        string Data;
                        while (canRead && (Data = Reader.ReadLine()) != null)
                        {
                            OnReceiveDataEvent(new IRCReadEventArgs(Data));
                        }
                    }).Start();
            }
            else
                throw new InvalidOperationException("Already reading!");
        }

        public void WriteLine(string Data, bool Flush = true)
        {
#           if DEBUG
            Core.Log("-> " + Data, Core.LogLevel.Debug);
#           endif

            Writer.WriteLine(Data);
            if (Flush)
                Writer.Flush();
        }

        protected virtual void OnReceiveDataEvent(IRCReadEventArgs e)
        {

            if (e.Split.Length > 3 &&
                e.Split[1] == "PRIVMSG" &&
                e.Split[3] == ":" + Program.C.Config.CommandPrefix + "help")
            {
                string[] split = e.Split;
                if (split.Length > 4)
                {
                    string Topic = string.Join(" ", split, 4, split.Length - 4);
                    OnReceiveHelpEvent(new IRCHelpEventArgs(Topic, split[2], e.Nick, e.User, e.Host));
                    return;
                }
                else
                    this.WriteLine("PRIVMSG " + split[2] + " :" + e.Nick + ", Usage: " + Program.C.Config.CommandPrefix + "help <topic>");
            }

            foreach (Tuple<string, string> T in Permissions.PermissionsTable)
            {
                if (Regex.IsMatch(e.Nick + "!" + e.User + "@" + e.Host, T.Item1))
                {
                    if (T.Item2 == "OP")
                        e.isOp = true;
                    if (T.Item2 == "ADMIN")
                        e.isAdmin = true;
                }
            }

            EventHandler<IRCReadEventArgs> Handler = OnReceiveData;

            // Event will be null if there are no subscribers 
            if (Handler != null)
            {
                //e.Message can be modified if we see fit

                Handler(this, e);
            }
        }

        protected virtual void OnReceiveHelpEvent(IRCHelpEventArgs e)
        {
            EventHandler<IRCHelpEventArgs> Handler = OnReceiveHelp;

            // Event will be null if there are no subscribers 
            if (Handler != null)
            {
                //e.Message can be modified if we see fit

                Handler(this, e);
            }
        }
    }
}
