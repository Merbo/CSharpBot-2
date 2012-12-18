using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class cmd_Quit : Module
    {
        public override int Run()
        {
            return MODULE_OKAY;
        }

        public override int SubscribeEvents()
        {
            return MODULE_OKAY;
        }

        public override int AddConfig()
        {
            return MODULE_OKAY;
        }

        public override void OnDataReceived(object sender, IRCReadEventArgs e)
        {
            if (e.Split.Length > 3 &&
                e.Split[1] == "PRIVMSG" &&
                e.Split[3] == ":" + Program.C.Config.CommandPrefix + "quit" &&
                e.isAdmin)
            {
                Connection C = (Connection)sender;
                if (e.Split.Length > 4)
                    C.WriteLine("QUIT :" + string.Join(" ", e.Split, 4, e.Split.Length - 4));
                else
                    C.WriteLine("QUIT :CSharpBot 2 -- https://github.com/Merbo/CSharpBot-2 http://173.48.92.80/CSharpBot");
            }
        }

        public override string GetName()
        {
            return "cmd_Quit";
        }

        public override string GetAuthor()
        {
            return "Merbo";
        }

        public override string GetDescription()
        {
            return "Adds command !quit [reason]" + Environment.NewLine +
                "Makes the bot quit for [reason]";
        }

        public override int OnTick()
        {
            return MODULE_OKAY;
        }

        public override void OnHelpReceived(object sender, IRCHelpEventArgs e)
        {
            if (e.Topic == "quit")
            {
                Connection C = (Connection)sender;
                C.WriteLine("PRIVMSG " + e.Target + " :" + e.Nick + ", " + Program.C.Config.CommandPrefix + "quit usage:");
                C.WriteLine("PRIVMSG " + e.Target + " :" + Program.C.Config.CommandPrefix + "quit [message]");
                C.WriteLine("PRIVMSG " + e.Target + " :Causes me to quit. [message] is optional; it is my quit message.");
            }
        }
    }
}
