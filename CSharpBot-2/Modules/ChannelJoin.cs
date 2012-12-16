using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class ChannelJoin : Module
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
            Connection C = (Connection)sender;
            if (e.Message.Contains("001 " + C.UserInfo.Nick + " :Welcome to the"))
            {
                C.WriteLine("JOIN #MerbosMagic");
            }
        }

        public override string GetName()
        {
            return "ChannelJoin";
        }

        public override string GetAuthor()
        {
            return "Merbo";
        }

        public override string GetDescription()
        {
            return "Incomplete module for allowing the bot to join channels.";
        }
    }
}
