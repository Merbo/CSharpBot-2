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
            Core.Log("Do you want do activate autorejoin plugin? [N] {Y, N, Yes, No}", Core.LogLevel.Config, false);

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
            if (Activate == true)
            {
                if (e.Message.Split(' ')[1] == "KICK")
                {
                    Connection conn = (Connection)sender;
                    conn.WriteLine("JOIN " + e.Message.Split(' ')[2]);
                }
            }
        }
    }
}
