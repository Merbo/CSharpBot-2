using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class ChannelJoin : Module
    {
        List<Tuple<Connection, List<string>>> ChannelConnectionTable;

        List<Tuple<Connection, bool>> JoinReadinessTable;

        public ChannelJoin()
        {
            ChannelConnectionTable = new List<Tuple<Connection, List<string>>>();
            JoinReadinessTable = new List<Tuple<Connection, bool>>();
        }

        public override int Run()
        {
            Core.Log("------------------CHANNELS-------------------", Core.LogLevel.Config);
            foreach (Connection C in Program.C.IrcManager.Connections)
            {
                List<string> Channels = new List<string>();
                bool JoinChans = true;
                Core.Log("Connection to " + C.Server, Core.LogLevel.Config);
                JoinChans:
                Core.Log("Do you want to join any channels? [Y] {Y, N, Yes, No} ", Core.LogLevel.Config, false);
                switch (Console.ReadLine().ToLower())
                {
                    case "y":
                    case "yes":
                    case "":
                        break;
                    case "n":
                    case "no":
                        JoinChans = false;
                        break;
                    default:
                        goto JoinChans;
                }
                if (!JoinChans)
                    continue;
                while (true)
                {
                    Core.Log("Channel name: ", Core.LogLevel.Config, false);
                    string channelName = Console.ReadLine();

                    if (channelName == "")
                        break;

                    if (!channelName.StartsWith("#"))
                    {
                        Core.Log("Channel name must begin with #", Core.LogLevel.Error);
                        continue;
                    }
                    if (!Channels.Contains(channelName.ToLower()))
                        Channels.Add(channelName.ToLower());
                    else
                    {
                        Core.Log("You've already added that chan!", Core.LogLevel.Error);
                        continue;
                    }
                }
                ChannelConnectionTable.Add(new Tuple<Connection, List<string>>(C, Channels));
            }

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
                JoinReadinessTable.Add(new Tuple<Connection, bool>(C, true));
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

        public override int OnTick()
        {
            foreach (Tuple<Connection, bool> JR in JoinReadinessTable)
            {
                if (JR.Item2)
                {
                    foreach (Tuple<Connection, List<string>> T in ChannelConnectionTable)
                    {
                        if (T.Item1 == JR.Item1)
                        {
                            foreach (string Channel in T.Item2)
                            {
                                T.Item1.WriteLine("JOIN " + Channel);
                            }
                        }
                    }
                    JoinReadinessTable.Remove(JR);
                }
            }

            return MODULE_OKAY;
        }
    }
}
