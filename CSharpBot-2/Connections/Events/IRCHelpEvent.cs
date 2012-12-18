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

        public string Topic
        {
            get
            {
                return msg;
            }
        }

        public string Target
        {
            get
            {
                return tgt;
            }
        }

        public string Nick
        {
            get
            {
                return nck;
            }
        }

        public string User
        {
            get
            {
                return usr;
            }
        }

        public string Host
        {
            get
            {
                return hst;
            }
        }
    }
}
