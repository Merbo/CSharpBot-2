using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class IRCUser
    {
        /// <summary>
        /// Nickname
        /// </summary>
        public readonly string Nick;

        /// <summary>
        /// User information (USER 0 * :Potato)
        /// </summary>
        public readonly string UserInfo;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nick">nickname</param>
        /// <param name="userinfo">user info for USER</param>
        public IRCUser(string nick, string userinfo)
        {
            Nick = nick;
            UserInfo = userinfo;
        }
    }
}
