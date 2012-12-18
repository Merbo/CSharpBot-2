using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class Permissions : Module
    {

        public static List<Tuple<string, string>> PermissionsTable;

        public override string GetName()
        {
            return "Permissions";
        }

        public override string GetAuthor()
        {
            return "Merbo";
        }

        public override string GetDescription()
        {
            return "Adds permissions system";
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
            PermissionsTable = new List<Tuple<string, string>>();

            Core.Log("----------------PERMISSIONS------------------", Core.LogLevel.Config);
            bool usePerms = true;
        usePerms:
            Core.Log("Do you want to set up permissions? [Yes/No]: ", Core.LogLevel.Config, false);
            switch (Console.ReadLine().ToLower())
            {
                case "y":
                case "yes":
                case "":
                    break;
                case "n":
                case "no":
                    usePerms = false;
                    break;
                default:
                    goto usePerms;
            }
            if (!usePerms)
                return MODULE_ERROR;

            bool keepAsking = true;
            Core.Log("Supply an empty value to break the loop!", Core.LogLevel.Config);
            while (keepAsking)
            {
                string Regex = "";
                string Access = "";

                Core.Log("What is the regex to match their hostmask? ", Core.LogLevel.Config, false);
                Regex = Console.ReadLine();
                if (Regex == "")
                {
                    keepAsking = false;
                    break;
                }

                Core.Log("What type of access do they get? [Admin/Op]: ", Core.LogLevel.Config, false);
                switch (Console.ReadLine().ToLower())
                {
                    case "a":
                    case "admin":
                        Access = "ADMIN";
                        break;
                    case "o":
                    case "op":
                        Access = "OP";
                        break;
                    default:
                        Core.Log("Invalid answer.", Core.LogLevel.Error);
                        continue;
                }

                Core.Log("REGEX = " + Regex, Core.LogLevel.Config);
                Core.Log("ACCESS = " + Access.ToUpper(), Core.LogLevel.Config);
                Core.Log("Verify this is correct. [Yes/No]: ", Core.LogLevel.Config, false);
                switch (Console.ReadLine().ToLower())
                {
                    case "y":
                    case "yes":
                    case "":
                        PermissionsTable.Add(new Tuple<string, string>(Regex, Access));
                        break;
                    case "n":
                    case "no":
                        continue;
                    default:
                        Core.Log("Invalid answer.", Core.LogLevel.Error);
                        continue;
                }
            }

            return MODULE_OKAY;
        }

        public override int OnTick()
        {
            return MODULE_OKAY;
        }

        public override void OnDataReceived(object sender, IRCReadEventArgs e)
        {
        }

        public override void OnHelpReceived(object sender, IRCHelpEventArgs e)
        {
            if (e.Topic == "permissions")
            {
                Connection C = (Connection)sender;
                C.WriteLine("PRIVMSG " + e.Target + " :" + e.Nick + ": Internal. Permissions system.");
            }
        }
    }
}
