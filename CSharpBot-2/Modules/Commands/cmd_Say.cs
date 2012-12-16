﻿using System;
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
                //:negus!negus@negus PRIVMSG #icebot :dicks lolololololololllololol
                //0                  1       2       3...
                if (split[2].StartsWith("#") && split.Length > 3)
                {
                    Connection C = (Connection)sender;
                    C.WriteLine("PRIVMSG " + split[2] + " :" + string.Join(" ", split, 4, split.Length - 4));
                }

            }
        }
    }
}
