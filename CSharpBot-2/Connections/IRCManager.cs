using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class IRCManager
    {
        public List<Connection> Connections;
        private Configuration Config;

        public event EventHandler<ConnectionAddedEventArgs> ConnectionAddedEvent;

        public IRCManager(Configuration config)
        {
            Connections = new List<Connection>();
            Config = config;
        }

        public void SetupConnections()
        {
            foreach (Tuple<string, int, IRCUser> Server in Config.Servers)
            {
                Connection connection = new Connection(Server.Item1, Server.Item2, Server.Item3);
                connection.BeginRead();
                Connections.Add(connection);
                OnConnectionAdded(new ConnectionAddedEventArgs("", connection));
            }
        }

        public void OnConnectionAdded(ConnectionAddedEventArgs e)
        {
            EventHandler<ConnectionAddedEventArgs> Handler = ConnectionAddedEvent;

            // Event will be null if there are no subscribers 
            if (Handler != null)
            {

                // Use the () operator to raise the event.
                Handler(this, e);
            }
        }
    }
}
