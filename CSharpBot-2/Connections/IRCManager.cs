using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class IRCManager
    {
        /// <summary>
        /// Our connections to manage
        /// </summary>
        public List<Connection> Connections;

        /// <summary>
        /// Our configuration
        /// </summary>
        private Configuration Config;

        /// <summary>
        /// What happens when a connection's added
        /// </summary>
        public event EventHandler<ConnectionAddedEventArgs> ConnectionAddedEvent;

        /// <summary>
        /// Setup IRC
        /// </summary>
        /// <param name="config">configuration</param>
        public IRCManager(Configuration config)
        {
            Connections = new List<Connection>();
            Config = config;
        }

        /// <summary>
        /// Setup all of our connections
        /// </summary>
        public void SetupConnections()
        {
            foreach (Tuple<string, int, IRCUser, bool> Server in Config.Servers)
            {
                Connection connection = new Connection(Server.Item1, Server.Item2, Server.Item3, Server.Item4);
                connection.BeginRead();
                Connections.Add(connection);
                OnConnectionAdded(new ConnectionAddedEventArgs("", connection));
            }
        }

        /// <summary>
        /// Called when a connection is added
        /// </summary>
        /// <param name="e"></param>
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
