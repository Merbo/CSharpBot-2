using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class Autorejoin : Module
    {
        Boolean Activate;
        public override string GetName()
        {
            return "Autorejoin";
        }

        public override string GetAuthor()
        {
            return "xaxes";
        }

        public override string GetDescription()
        {
            return "Auto join to channel after kick";
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
            Core.Log("-----------------AUTOREJOIN------------------", Core.LogLevel.Config);
            Core.Log("Do you want do activate autorejoin plugin? [N] {Y, N, Yes, No} ", Core.LogLevel.Config, false);

            switch (Console.ReadLine().ToLower())
            {
                case "y":
                case "yes":
                    Activate = true;
                    break;
                case "n":
                case "no":
                case "":
                    Activate = false;
                    break;
                default:
                    Activate = false;
                    break;
            }

            return 0;
        }

        public override int OnTick()
        {
            return MODULE_OKAY;
        }

        public override void OnDataReceived(object sender, IRCReadEventArgs e)
        {
            Connection C = (Connection)sender;
            string[] split = e.Split;

            if (split.Length > 5 && 
                split[4] == ":autorejoin" && 
                split[5] == "on")
            {
                Activate = true;
                Core.Log("Autorejoin turned on", Core.LogLevel.Info);
                C.WriteLine("PRIVMSG " + split[2] + " :" + e.Nick + ", " + "Autorejoin turned on");
            }
            if (split.Length > 5 &&
                split[4] == ":autorejoin" &&
                split[5] == "off")
            {
                Activate = false;
                Core.Log("Autorejoin turned off", Core.LogLevel.Info);
                C.WriteLine("PRIVMSG " + split[2] + " :" + e.Nick + ", " + "Autorejoin turned off");
            }
            if (Activate == true)
            {
                if (split[1] == "KICK")
                {
                    C.WriteLine("JOIN " + split[2]);
                }
            }
        }

        public override void OnHelpReceived(object sender, IRCHelpEventArgs e)
        {
            if (e.Topic == "autorejoin")
            {
                Connection C = (Connection)sender;
                C.WriteLine("PRIVMSG " + e.Target + " :" + e.Nick + ", " + Program.C.Config.CommandPrefix + "Autorejoin usage:");
                C.WriteLine("PRIVMSG " + e.Target + " :" + Program.C.Config.CommandPrefix + "autorejoin <on|off>");
            }
        }
    }
}
