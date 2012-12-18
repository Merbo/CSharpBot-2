using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class cmd_Ls : Module
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
            if (e.Split.Length > 3 &&
                e.Split[1] == "PRIVMSG" &&
                e.Split[3] == ":" + Program.C.Config.CommandPrefix + "ls")
            {
                string Modules = "";
                foreach (Module M in Program.Modules)
                {
                    Modules += M.GetName() + ", ";
                }
                Modules = Modules.Remove(Modules.Length - 2, 2);
                Modules += ".";
                Connection C = (Connection)sender;
                C.WriteLine("PRIVMSG " + e.Split[2] + " :" + e.Nick + ": " + Modules);
            }
        }

        public override void OnHelpReceived(object sender, IRCHelpEventArgs e)
        {
            if (e.Topic == "ls")
            {
                Connection C = (Connection)sender;
                C.WriteLine("PRIVMSG " + e.Target + " :" + e.Nick + ", " + Program.C.Config.CommandPrefix + "ls usage:");
                C.WriteLine("PRIVMSG " + e.Target + " :" + Program.C.Config.CommandPrefix + "ls");
                C.WriteLine("PRIVMSG " + e.Target + " :Causes me to list off my loaded modules.");
            }
        }
    }
}
