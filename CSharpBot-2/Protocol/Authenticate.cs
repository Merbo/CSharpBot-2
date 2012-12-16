using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class Authenticate : Module
    {
        public override int Run()
        {
            Program.C.IrcManager.ConnectionAddedEvent += new EventHandler<ConnectionAddedEventArgs>(this.Auth);
            return MODULE_OKAY;
        }

        public override int Init()
        {
            return MODULE_OKAY;
        }

        private void Auth(object sender, ConnectionAddedEventArgs e)
        {
            if (e != null)
            {
                Connection C = e.Connection;

                if (C != null)
                {
                    C.WriteLine("NICK " + C.UserInfo.Nick);
                    C.WriteLine("USER " + C.UserInfo.Nick + " 0 * :" + C.UserInfo.UserInfo);
                }
            }
        }
    }
}
