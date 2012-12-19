using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    public class IRCHelpEventArgs : EventArgs
    {
        public IRCHelpEventArgs(string Msg, string Tgt, string Nck, string Usr, string Hst)
        {
            msg = Msg;
            tgt = Tgt;
            nck = Nck;
            usr = Usr;
            hst = Hst;

            if (msg.StartsWith(Program.C.Config.CommandPrefix))
                msg = msg.Remove(0, Program.C.Config.CommandPrefix.Length);

            msg = msg.ToLower();
        }

        private string msg;
        private string tgt;
        private string nck;
        private string usr;
        private string hst;

        /// <summary>
        /// Help topic. Always in lowercase and has no command prefix
        /// </summary>
        public string Topic
        {
            get
            {
                return msg;
            }
        }

        /// <summary>
        /// calling target (chan/nick)
        /// </summary>
        public string Target
        {
            get
            {
                return tgt;
            }
        }

        /// <summary>
        /// Caller
        /// </summary>
        public string Nick
        {
            get
            {
                return nck;
            }
        }

        /// <summary>
        /// Caller
        /// </summary>
        public string User
        {
            get
            {
                return usr;
            }
        }

        /// <summary>
        /// Caller
        /// </summary>
        public string Host
        {
            get
            {
                return hst;
            }
        }
    }
}
