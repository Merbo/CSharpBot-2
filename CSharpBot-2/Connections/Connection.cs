using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace CSharpBot
{
    class Connection
    {
        private TcpClient IRC;
        private Stream IRCStream;
        private StreamWriter Writer;
        private StreamReader Reader;

        public bool canRead;
        private bool isReading;

        public IRCUser UserInfo;

        public event EventHandler<IRCReadEventArgs> OnReceiveData;

        public Connection(string Server, int Port, IRCUser userInfo)
        {
            IRC = new TcpClient(Server, Port);
            IRCStream = IRC.GetStream();
            Reader = new StreamReader(IRCStream);
            Writer = new StreamWriter(IRCStream);

            UserInfo = userInfo;

            isReading = false;
            canRead = true;
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
            EventHandler<IRCReadEventArgs> Handler = OnReceiveData;

            // Event will be null if there are no subscribers 
            if (Handler != null)
            {
                //e.Message can be modified if we see fit

                Handler(this, e);
            }
        }
    }
}
