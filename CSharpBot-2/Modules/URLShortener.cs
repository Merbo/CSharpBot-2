using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharpBot.Modules
{
    class URLShortener : Module
    {
        Boolean Activate;
        public override string GetName()
        {
            return "URL Shortner";
        }

        public override string GetAuthor()
        {
            return "xaxes";
        }

        public override string GetDescription()
        {
            return "Shorten every URL sent to channel";
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
            Core.Log("----------------URLSHORTENER-----------------", Core.LogLevel.Config);
            Core.Log("Do you want do activate URLShortener plugin? [N] {Y, N, Yes, No} ", Core.LogLevel.Config, false);

            switch (Console.ReadLine().ToLower())
            {
                case "y":
                case "yes":
                    Activate = true;
                    break;
                case "n":
                case "no":
                case "":
                    Activate = false;
                    break;
                default:
                    Activate = false;
                    break;
            }

            return 0;
        }

        public override int OnTick()
        {
            return MODULE_OKAY;
        }

        public override void OnDataReceived(object sender, IRCReadEventArgs e)
        {
            if (Activate)
            {
                if (e.Message.Contains("PRIVMSG") &&
                        Regex.IsMatch(e.Message, @".+http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\0'\/\\\+&amp;%\$#_]*)?$", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                {
                    Connection C = (Connection)sender;
                    WebClient client = new WebClient();
                    List<string> URLs = new List<string>();
                    List<string> shortened = new List<string>();

                    foreach (var match in new Regex(@"http(s?)://\S+").Matches(e.Message)) URLs.Add(match.ToString());
                    try
                    {
                        foreach (string URL in URLs) shortened.Add(client.DownloadString("http://tinyurl.com/api-create.php?url=" + URL));
                    }
                    catch (WebException)
                    {
                        C.WriteLine("PRIVMSG " + e.Message.Split(' ')[2] + " :There was an error accessing tinyurl's API.");
                        return;
                    }
                    C.WriteLine("PRIVMSG " + e.Message.Split(' ')[2] + " :·· " + string.Join(", ", shortened));
                }
            }
        }

        public override void OnHelpReceived(object sender, IRCHelpEventArgs e)
        {
            if (e.Topic == "urlshortener")
            {
                Connection C = (Connection)sender;
                C.WriteLine("PRIVMSG " + e.Target + " :" + e.Nick + " :Shortens every link sent do channel. Uses tinyurl's API.");
            }
        }
    }
}
