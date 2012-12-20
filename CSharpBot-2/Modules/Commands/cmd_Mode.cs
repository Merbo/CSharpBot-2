using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class cmd_Mode : Module
    {
        public override string GetName()
        {
            return "cmd_Mode";
        }

        public override string GetAuthor()
        {
            return "xaxes";
        }

        public override string GetDescription()
        {
            return "Sets mode of channel/user";
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

        public override void OnDataReceived(object sender, IRCReadEventArgs e)
        {
            if (e.Split.Length > 4 && 
                e.Split[1] == "PRIVMSG" &&
                e.Split[3] == ":" + Program.C.Config.CommandPrefix + "mode" &&
                e.isOp ||
                e.isAdmin)
            {
                string[] split = e.Message.Split(' ');
                if (split[2].StartsWith("#"))
                {
                    Connection C = (Connection)sender;
                    string Params = string.Join(" ", e.Split, 4, e.Split.Length - 4);
                    C.WriteLine("MODE " + e.Split[2] + " :" + Params);
                }
            }
        }

        public override int OnTick()
        {
            return MODULE_OKAY;
        }

        public override void OnHelpReceived(object sender, IRCHelpEventArgs e)
        {
            if (e.Topic == "mode")
            {
                Connection C = (Connection)sender;
                C.WriteLine("PRIVMSG " + e.Target + " :" + e.Nick + ", " + Program.C.Config.CommandPrefix + "mode usage:");
                C.WriteLine("PRIVMSG " + e.Target + " :" + Program.C.Config.CommandPrefix + "mode <mode>");
                C.WriteLine("PRIVMSG " + e.Target + " :Causes me to change the current channel's modes. <mode> is the modes to apply.");
            }
        }
    }
}
