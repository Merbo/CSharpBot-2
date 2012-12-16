using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class IRCUser
    {
        public readonly string Nick;
        public readonly string UserInfo;
        public IRCUser(string nick, string userinfo)
        {
            Nick = nick;
            UserInfo = userinfo;
        }
    }
}
