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
        /// <summary>
        /// Our IRC base connection
        /// </summary>
        private TcpClient IRC;

        /// <summary>
        /// stream for user
        /// </summary>
        private Stream IRCStream;

        /// <summary>
        /// StreamWriter for irc
        /// </summary>
        private StreamWriter Writer;

        /// <summary>
        /// Streamreader for irc
        /// </summary>
        private StreamReader Reader;

        /// <summary>
        /// server to connect to
        /// </summary>
        public string Server;
        
        /// <summary>
        /// Port to connect on
        /// </summary>
        public int Port;

        /// <summary>
        /// Can we continue reading?
        /// </summary>
        public bool canRead;

        /// <summary>
        /// Are we reading?
        /// </summary>
        private bool isReading;

        /// <summary>
        /// User info of the bot's connection here
        /// </summary>
        public IRCUser UserInfo;

        /// <summary>
        /// Starts when we get data
        /// </summary>
        public event EventHandler<IRCReadEventArgs> OnReceiveData;

        /// <summary>
        /// Starts when we get a help request
        /// </summary>
        public event EventHandler<IRCHelpEventArgs> OnReceiveHelp;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="server">Server</param>
        /// <param name="port">Port</param>
        /// <param name="userInfo">UserInfo</param>
        /// <param name="ssl">Use a secure connection?</param>
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

        /// <summary>
        /// All servers are usable for a connection :)
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="Cert">certificate</param>
        /// <param name="Chain">cert chain</param>
        /// <param name="Errors">certificate errors?</param>
        /// <returns></returns>
        public bool ValidateServerCert(object sender, X509Certificate Cert, X509Chain Chain, SslPolicyErrors Errors)
        {
            return true;
        }

        /// <summary>
        /// Ensure what local cert we can use. INVALID AS OF YET
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="name"></param>
        /// <param name="Collection"></param>
        /// <param name="Cert"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public X509Certificate ValidateLocalCert(object sender, string name, X509CertificateCollection Collection, X509Certificate Cert, string[] names)
        {
            return null;
        }

        /// <summary>
        /// Makes a new thread and begins reading from the IRC connection
        /// </summary>
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

        /// <summary>
        /// Writes to the IRC connection
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Flush"></param>
        public void WriteLine(string Data, bool Flush = true)
        {
#           if DEBUG
            Core.Log("-> " + Data, Core.LogLevel.Debug);
#           endif

            Writer.WriteLine(Data);
            if (Flush)
                Writer.Flush();
        }

        /// <summary>
        /// What happens when we get data from IRC?
        /// </summary>
        /// <param name="e"></param>
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

        /// <summary>
        /// What happens when we get a help request?
        /// </summary>
        /// <param name="e"></param>
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
