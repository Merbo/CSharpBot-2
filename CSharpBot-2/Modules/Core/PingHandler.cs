using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class PingHandler : Module
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
            if (e.Message.StartsWith("PING :"))
            {
                Connection C = (Connection)sender;
                string[] PingData = e.Message.Split(':');
                C.WriteLine("PONG :" + PingData[1]);
            }
        }

        public override string GetName()
        {
            return "PingHandler";
        }

        public override string GetAuthor()
        {
            return "Merbo";
        }

        public override string GetDescription()
        {
            return "Allows the bot to stay alive by responding to pings.";
        }

        public override int OnTick()
        {
            return MODULE_OKAY;
        }
    }
}
