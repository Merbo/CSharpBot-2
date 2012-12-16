using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class Authenticate : Module
    {
        public void Run()
        {
            Program.C.IrcManager.ConnectionAddedEvent += delegate (object sender, ConnectionAddedEventArgs e)
            {
                Connection C = e.Connection;

                C.WriteLine("NICK " + C.UserInfo.Nick);
                C.WriteLine("USER " + C.UserInfo.Nick + " 0 * :" + C.UserInfo.UserInfo);
            };
        }
    }
}
