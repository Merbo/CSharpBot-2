using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;

namespace CSharpBot
{
    class YoutubeResolve : Module
    {
        public override string GetName()
        {
            return "YoutubeResolve";
        }

        public override string GetAuthor()
        {
            return "Merbo";
        }

        public override string GetDescription()
        {
            return "Resolves youtube links and grabs their title";
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
            if (e.Message.Contains("PRIVMSG") &&
                Regex.IsMatch(e.Message, @".+http://(?:www\.)?youtu(?:be\.com/watch\?v=|\.be/)(\w*)(&(amp;)?[\w\?=]*)?.+"))
            {
                Connection Conn = (Connection)sender;
                Match match = Regex.Match(e.Message, @".+http://(?:www\.)?youtu(?:be\.com/watch\?v=|\.be/)(\w*)(&(amp;)?[\w\?=]*)?.+");
                string videoid = match.Groups[1].Value;
                if (videoid.Contains(".be"))
                    videoid = match.Groups[2].Value;
                WebClient client = new WebClient();
                string source = client.DownloadString("http://gdata.youtube.com/feeds/api/videos/" + videoid);
                Match titleMatch = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase);
                string title = titleMatch.Groups["Title"].Value;

                Conn.WriteLine("PRIVMSG " + e.Message.Split(' ')[2] + " :YouTube video title is \"" + title + "\"");
            }
        }
    }
}
