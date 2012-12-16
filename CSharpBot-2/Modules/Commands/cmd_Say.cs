using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class cmd_Say : Module
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
            if (e.Message.Contains("PRIVMSG ") &&
                e.Message.Contains(Program.C.Config.CommandPrefix + "say"))
            {
                string[] split = e.Message.Split(' ');
                if (split[2].StartsWith("#") && split.Length > 3)
                {
                    Connection C = (Connection)sender;
                    C.WriteLine("PRIVMSG " + split[2] + " :" + string.Join(" ", split, 4, split.Length - 4));
                }

            }
        }

        public override string GetName()
        {
            return "cmd_Say";
        }

        public override string GetAuthor()
        {
            return "Merbo";
        }

        public override string GetDescription()
        {
            return "Adds command !say <text>" + Environment.NewLine +
                "Makes the bot say <text>";
        }

        public override int OnTick()
        {
            return MODULE_OKAY;
        }
    }
}
