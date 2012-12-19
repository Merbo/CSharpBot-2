using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    public class IRCReadEventArgs : EventArgs
    {

        private string msg;
        private string[] split;
        private string user = null;
        private string ident = null;
        private string host = null;
        private bool admin = false;
        private bool op = false;

        public IRCReadEventArgs(string FullMsg)
        {
            msg = FullMsg;
            split = msg.Split(' ');
            if (split.Length > 1)
                if (split[1] == "PRIVMSG" ||
                    split[1] == "NOTICE" && split[2] != "*" ||
                    split[1] == "MODE" ||
                    split[1] == "TOPIC" || 
                    split[1] == "JOIN" ||
                    split[1] == "PART" ||
                    split[1] == "QUIT")
                {
                    if (split[0].Contains('!') &&  
                        split[0].Contains('@'))
                    {
                        string FullTitle = split[0].Remove(0, 1);
                        user = FullTitle.Split('!')[0];
                        host = FullTitle.Split('@')[1];
                        ident = FullTitle.Remove(0, 1)
                                        .Remove(0, user.Length);
                        ident = ident.Remove(ident.Length - host.Length, host.Length);
                    }
                }
        }

        /// <summary>
        /// Full IRC RAW
        /// </summary>
        public string Message
        {
            get 
            { 
                return msg; 
            }
        }

        /// <summary>
        /// array, separated by space, of words in Message.
        /// </summary>
        public string[] Split
        {
            get
            {
                return split;
            }
        }

        /// <summary>
        /// MAY BE NULL. Nickname of caller
        /// </summary>
        public string Nick
        {
            get
            {
                return user;
            }
        }

        /// <summary>
        /// MAY BE NULL. Username of caller
        /// </summary>
        public string User
        {
            get
            {
                return ident;
            }
        }

        /// <summary>
        /// MAY BE NULL. Hostname of caller
        /// </summary>
        public string Host
        {
            get
            {
                return host;
            }
        }

        /// <summary>
        /// MAY BE NULL. If the caller is an admin
        /// </summary>
        public bool isAdmin
        {
            get
            {
                return admin;
            }
            set
            {
                admin = value;
            }
        }

        /// <summary>
        /// MAY BE NULL. If the caller is an op
        /// </summary>
        public bool isOp
        {
            get
            {
                return op;
            }
            set
            {
                op = value;
            }
        }
    }
}
