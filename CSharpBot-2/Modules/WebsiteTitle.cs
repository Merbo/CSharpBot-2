using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharpBot.Modules
{
    class WebsiteTitle : Module
    {
        Boolean Activate;
        public override string GetName()
        {
            return "WebsiteTitle";
        }

        public override string GetAuthor()
        {
            return "xaxes";
        }

        public override string GetDescription()
        {
            return "Sends to channel a title of website from URL";
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
            Core.Log("----------------WEBSITETITLE-----------------", Core.LogLevel.Config);
            Core.Log("Do you want do activate WebsiteTitle plugin? [N] {Y, N, Yes, No} ", Core.LogLevel.Config, false);

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
                    !Regex.IsMatch(e.Message, @".+https?://(?:www\.)?youtu(?:be\.com/watch\?v=|\.be/)(\w*)(&(amp;)?[\w\?=]*)?") &&
                    Regex.IsMatch(e.Message, @".+http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\0'\/\\\+&amp;%\$#_]*)?$", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                {
                    Connection C = (Connection)sender;
                    WebClient client = new WebClient();
                    string source = e.Message;
                    List<string> URLs = new List<string>();

                    foreach (var match in new Regex(@"http(s?)://\S+").Matches(source)) URLs.Add(match.ToString());
                    foreach (string URL in URLs)
                    {
                        try
                        {
                            source = client.DownloadString(URL);
                        }
                        catch (WebException)
                        {
                            C.WriteLine("PRIVMSG " + e.Message.Split(' ')[2] + " :There was an error fetching website title.");
                            return;
                        }
                        Match titleMatch = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase);
                        string title = titleMatch.Groups["Title"].Value;
                        C.WriteLine("PRIVMSG " + e.Message.Split(' ')[2] + " :·· " + title);
                    }
                }
            }
        }

        public override void OnHelpReceived(object sender, IRCHelpEventArgs e)
        {
            if (e.Topic == "websitetitle")
            {
                Connection C = (Connection)sender;
                C.WriteLine("PRIVMSG " + e.Target + " :" + e.Nick + " :Sends to channel a title of website sent to channel."); // Yo dawg
            }
        }
    }
}
