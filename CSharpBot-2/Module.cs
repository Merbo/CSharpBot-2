using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    public abstract class Module
    {

        public const int MODULE_OKAY  = 0;
        public const int MODULE_ERROR = 1;
        public const int MODULE_FATAL = 2;

        public Module()
        {
        }
        ~Module()
        {
            if (Program.C != null)
                if (Program.C.IrcManager != null)
                    foreach (Connection C in Program.C.IrcManager.Connections)
                        C.OnReceiveData -= OnDataReceived;
        }
        public abstract int Run();

        public abstract int SubscribeEvents();

        public virtual int SubscribeEventRead()
        {
            if (Program.C != null)
                if (Program.C.IrcManager != null)
                    foreach (Connection C in Program.C.IrcManager.Connections)
                        C.OnReceiveData += OnDataReceived;

            return MODULE_OKAY;
        }

        public abstract int AddConfig();

        public abstract void OnDataReceived(object sender, IRCReadEventArgs e);
    }
}
