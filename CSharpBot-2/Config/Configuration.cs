using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class Configuration
    {
        public List<Tuple<string, int, IRCUser>> Servers;

        public Configuration()
        {
            Servers = new List<Tuple<string, int, IRCUser>>();
        }

        public void RunInteractiveConfigWizard()
        {
            Core.Log("----------CSharpBot-2 Configuration----------", Core.LogLevel.Config);
            Core.Log("-------------------SERVERS-------------------", Core.LogLevel.Config);
            Core.Log("  Let's set up the first server, shall we?", Core.LogLevel.Config);
            AddServer();
        }

        public void AddServer()
        {
            string Hostname;
            int Port;
            string Nick;
            string UserInfo;
            Core.Log("-----------------NEW SERVER------------------", Core.LogLevel.Config);
        //Hostname
            Core.Log("Server Hostname [173.48.92.80]:", Core.LogLevel.Config, false);
            Hostname = Console.ReadLine();
            if (Hostname == "")
                Hostname = "173.48.92.80";
        Port:
            Core.Log("Server Port [6667]:", Core.LogLevel.Config, false);
            string tmp = Console.ReadLine();
            if (tmp == "")
                tmp = "6667";
            if (!int.TryParse(tmp, out Port) || Port < 1 || Port > 65535)
            {
                Core.Log("That port number is unacceptable.", Core.LogLevel.Error);
                goto Port;
            }
        //Nick
            Core.Log("Nick [CSharpBot-2]:", Core.LogLevel.Config, false);
            Nick = Console.ReadLine();
            if (Nick == "")
                Nick = "CSharpBot-2";
        //Userinfo
            Core.Log("User [MerbosMagic CSharpBot 2]:", Core.LogLevel.Config, false);
            UserInfo = Console.ReadLine();
            if (UserInfo == "")
                UserInfo = "MerbosMagic CSharpBot 2";

            Servers.Add(new Tuple<string, int, IRCUser>(Hostname, Port, new IRCUser(Nick, UserInfo)));
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
