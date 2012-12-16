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
            return MODULE_OKAY;
        }

        public override int AddConfig()
        {
            return MODULE_OKAY;
        }

        public override int SubscribeEvents()
        {
            if (Program.C != null)
                if (Program.C.IrcManager != null)
                    Program.C.IrcManager.ConnectionAddedEvent += this.Auth;

            return MODULE_OKAY;
        }

        ~Authenticate()
        {
            Program.C.IrcManager.ConnectionAddedEvent -= this.Auth;
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

        public override void OnDataReceived(object sender, IRCReadEventArgs e)
        {
        }

        public override string GetName()
        {
            return "Authenticate";
        }

        public override string GetAuthor()
        {
            return "Merbo";
        }

        public override string GetDescription()
        {
            return "Allows the bot to send NICK and USER to connect to the IRC Server.";
        }
    }
}
