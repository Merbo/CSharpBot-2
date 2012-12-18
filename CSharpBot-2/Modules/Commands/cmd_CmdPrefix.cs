using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class cmd_CmdPrefix : Module
    {
        public override string GetName()
        {
            return "cmd_Ls";
        }

        public override string GetAuthor()
        {
            return "Merbo";
        }

        public override string GetDescription()
        {
            return "Lists available modules.";
        }

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

        public override int OnTick()
        {
            return MODULE_OKAY;
        }

        public override void OnDataReceived(object sender, IRCReadEventArgs e)
        {
            if (e.Split.Length > 4 &&
                e.Split[1] == "PRIVMSG" &&
                e.Split[3] == ":" + Program.C.Config.CommandPrefix + "cmdprefix")
            {
                Connection C = (Connection)sender;
                Program.C.Config.CommandPrefix = e.Split[4];
                C.WriteLine("PRIVMSG " + e.Split[2] + " :" + e.Nick + ": cmd prefix set to " + e.Split[4]);
            }
        }

        public override void OnHelpReceived(object sender, IRCHelpEventArgs e)
        {
            if (e.Topic == "cmdprefix")
            {
                Connection C = (Connection)sender;
                C.WriteLine("PRIVMSG " + e.Target + " :" + e.Nick + ", " + Program.C.Config.CommandPrefix + "cmdprefix usage:");
                C.WriteLine("PRIVMSG " + e.Target + " :" + Program.C.Config.CommandPrefix + "cmdprefix <prefix>");
                C.WriteLine("PRIVMSG " + e.Target + " :Changes my command prefix. <prefix> is the prefix (no spaces)");
            }
        }
    }
}
