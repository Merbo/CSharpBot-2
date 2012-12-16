using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class ConnectionAddedEventArgs : EventArgs
    {
        public ConnectionAddedEventArgs(string s, Connection c)
        {
            msg = s;
            connection = c;
        }
       
        private string msg;
        private Connection connection;

        public string Message
        {
            get 
            { 
                return msg; 
            }
            set
            {
                msg = value;
            }
        }

        public Connection Connection
        {
            get
            {
                return connection;
            }
        }
    }
}
