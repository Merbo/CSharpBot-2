﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class cmd_Topic : Module
    {
        public override string GetName()
        {
            return "cmd_Topic";
        }

        public override string GetAuthor()
        {
            return "Merbo";
        }

        public override string GetDescription()
        {
            return "Adds !topic <topic>" + Environment.NewLine + 
                "Allows modification of channel topic";
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

        public override int OnTick()
        {
            return MODULE_OKAY;
        }

        public override void OnDataReceived(object sender, IRCReadEventArgs e)
        {
            if (e.Message.Contains("PRIVMSG ") &&
                e.Message.Contains(Program.C.Config.CommandPrefix + "topic"))
            {
                string[] split = e.Split;
                if (split[2].StartsWith("#") && split.Length > 3)
                {
                    Connection C = (Connection)sender;
                    C.WriteLine("TOPIC " + split[2] + " :" + string.Join(" ", split, 4, split.Length - 4));
                }

            }
        }

        public override void OnHelpReceived(object sender, IRCHelpEventArgs e)
        {
            if (e.Topic == "topic")
            {
                Connection C = (Connection)sender;
                C.WriteLine("PRIVMSG " + e.Target + " :" + e.Nick + ", " + Program.C.Config.CommandPrefix + "topic usage:");
                C.WriteLine("PRIVMSG " + e.Target + " :" + Program.C.Config.CommandPrefix + "topic <topic>");
                C.WriteLine("PRIVMSG " + e.Target + " :Causes me to change the current channel's topic. <topic> is the topic.");
            }
        }
    }
}
