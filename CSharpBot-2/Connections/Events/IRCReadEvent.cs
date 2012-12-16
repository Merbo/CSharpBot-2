using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class IRCReadEventArgs : EventArgs
    {
        public IRCReadEventArgs(string s)
        {
            msg = s;
        }
       
        private string msg;

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
    }
}
