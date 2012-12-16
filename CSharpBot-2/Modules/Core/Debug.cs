using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot.Modules
{
    class Debug : Module
    {
        public override int Run()
        {
            return MODULE_OKAY;
        }

        public override int AddConfig()
        {
            return MODULE_OKAY;
        }

        public override void OnDataReceived(object sender, IRCReadEventArgs e)
        {
#           if DEBUG
            Core.Log(e.Message, Core.LogLevel.Debug);
#           endif
        }

        public override int SubscribeEvents()
        {
            return MODULE_OKAY;
        }
    }
}
