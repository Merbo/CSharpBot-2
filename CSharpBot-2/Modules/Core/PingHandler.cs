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
    }
}
