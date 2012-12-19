using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class Configuration
    {
        /// <summary>
        /// hostname, port, ircuser info, ssl?
        /// </summary>
        public List<Tuple<string, int, IRCUser, bool>> Servers;

        /// <summary>
        /// Command prefix
        /// </summary>
        public string CommandPrefix;

        public Configuration()
        {
            Servers = new List<Tuple<string, int, IRCUser, bool>>();
            CommandPrefix = "!";
        }

        /// <summary>
        /// Begin config
        /// </summary>
        public void RunInteractiveConfigWizard()
        {
            Core.Log("----------CSharpBot-2 Configuration----------", Core.LogLevel.Config);
            Core.Log("---------------COMMAND-PREFIX----------------", Core.LogLevel.Config);
            Core.Log("What do you want your command prefix to be? [" + CommandPrefix + "] ", Core.LogLevel.Config, false);
            CommandPrefix = Console.ReadLine();
            if (CommandPrefix == "")
                CommandPrefix = "!";
            Core.Log("-------------------SERVERS-------------------", Core.LogLevel.Config);
            Core.Log("  Let's set up the first server, shall we?", Core.LogLevel.Config);
            AddServer();
        }

        /// <summary>
        /// Add a server
        /// </summary>
        public void AddServer()
        {
            string Hostname;
            int Port;
            string Nick;
            string UserInfo;
            Core.Log("-----------------NEW SERVER------------------", Core.LogLevel.Config);
        //Hostname
            Core.Log("Server Hostname [173.48.92.80]: ", Core.LogLevel.Config, false);
            Hostname = Console.ReadLine();
            if (Hostname == "")
                Hostname = "173.48.92.80";
        Port:
            Core.Log("Server Port [6667]: ", Core.LogLevel.Config, false);
            string tmp = Console.ReadLine();
            if (tmp == "")
                tmp = "6667";
            if (!int.TryParse(tmp, out Port) || Port < 1 || Port > 65535)
            {
                Core.Log("That port number is unacceptable.", Core.LogLevel.Error);
                goto Port;
            }
        //Nick
            Core.Log("Nick [CSharpBot-2]: ", Core.LogLevel.Config, false);
            Nick = Console.ReadLine();
            if (Nick == "")
                Nick = "CSharpBot-2";
        //Userinfo
            Core.Log("User [MerbosMagic CSharpBot 2]: ", Core.LogLevel.Config, false);
            UserInfo = Console.ReadLine();
            if (UserInfo == "")
                UserInfo = "MerbosMagic CSharpBot 2";

            bool SSL;
        SSL:
            Core.Log("Do you want SSL? [N] {Y, N, Yes, No} ", Core.LogLevel.Config, false);
            switch (Console.ReadLine().ToLower())
            {
                case "y":
                case "yes":
                    SSL = true;
                    break;
                case "n":
                case "no":
                case "":
                    SSL = false;
                    break;
                default:
                    goto SSL;
            }

            Servers.Add(new Tuple<string, int, IRCUser, bool>(Hostname, Port, new IRCUser(Nick, UserInfo), SSL));
        AddNewServer:
            Core.Log("Do you want to add another server? [N] {Y, N, Yes, No} ", Core.LogLevel.Config, false);
            switch (Console.ReadLine().ToLower())
            {
                case "y":
                case "yes":
                    AddServer();
                    break;
                case "n":
                case "no":
                case "":
                    break;
                default:
                    goto AddNewServer;
            }
        }
    }
}
