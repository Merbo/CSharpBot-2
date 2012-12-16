using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class Mode : Module
    {
        public override string GetName()
        {
            return "Mode";
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
            return Module.MODULE_OKAY;
        }

        public override int SubscribeEvents()
        {
            return Module.MODULE_OKAY;
        }

        public override int AddConfig()
        {
            return Module.MODULE_OKAY;
        }

        public override void OnDataReceived(object sender, IRCReadEventArgs e)
        {
            if (e.Message.Contains("PRIVMSG") &&
                e.Message.Contains(":" + Program.C.Config.CommandPrefix + "mode"))
            {
                Connection Conn = (Connection)sender;
                string[] Split = e.Message.Split(' ');
                string UserMess = e.Message.Split(new string[] { " :" }, StringSplitOptions.None)[1];
                string Chan = Split[2];
                string Mode = UserMess.Split(' ')[1];
                string Who = UserMess.Split(' ')[2];

                Conn.WriteLine("MODE " + Chan + " " + Mode + " " + Who);

                //Conn.WriteLine("PRIVMSG " + Chan + " :  " + UserMess);
            }
        }

        public override int OnTick()
        {
            return MODULE_OKAY;
        }
    }
}
