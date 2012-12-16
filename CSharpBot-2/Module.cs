using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    public abstract class Module
    {
        /// <summary>
        /// Module did not encounter any errors
        /// </summary>
        public const int MODULE_OKAY  = 0;

        /// <summary>
        /// Minor error, report it
        /// </summary>
        public const int MODULE_ERROR = 1;

        /// <summary>
        /// Fatal error, kill bot now
        /// </summary>
        public const int MODULE_FATAL = 2;

        /// <summary>
        /// Get the name of the module
        /// </summary>
        /// <returns>The name of the module</returns>
        public abstract string GetName();

        /// <summary>
        /// Get the author of the module
        /// </summary>
        /// <returns>The author of the module</returns>
        public abstract string GetAuthor();

        /// <summary>
        /// Get the description of the module
        /// </summary>
        /// <returns>A description of the module. May be multiline.</returns>
        public abstract string GetDescription();

        /// <summary>
        /// Generic constructor
        /// </summary>
        public Module()
        {
        }

        /// <summary>
        /// Generic deconstructor
        /// </summary>
        ~Module()
        {
            if (Program.C != null)
                if (Program.C.IrcManager != null)
                    foreach (Connection C in Program.C.IrcManager.Connections)
                        C.OnReceiveData -= OnDataReceived;
        }

        /// <summary>
        /// Called after connections are setup and bot is initialized
        /// </summary>
        /// <returns>A const int</returns>
        /// <seealso cref="MODULE_OKAY"/>
        /// <seealso cref="MODULE_ERROR"/>
        /// <seealso cref="MODULE_FATAL"/>
        public abstract int Run();

        /// <summary>
        /// Called before connections are setup but after bot is initialized
        /// </summary>
        /// <returns>A const int</returns>
        /// <seealso cref="MODULE_OKAY"/>
        /// <seealso cref="MODULE_ERROR"/>
        /// <seealso cref="MODULE_FATAL"/>
        public abstract int SubscribeEvents();

        /// <summary>
        /// Called internally, do not use or override
        /// </summary>
        /// <returns>A const int</returns>
        /// <seealso cref="MODULE_OKAY"/>
        /// <seealso cref="MODULE_ERROR"/>
        /// <seealso cref="MODULE_FATAL"/>
        public virtual int SubscribeEventRead()
        {
            if (Program.C != null)
                if (Program.C.IrcManager != null)
                    foreach (Connection C in Program.C.IrcManager.Connections)
                        C.OnReceiveData += OnDataReceived;

            return MODULE_OKAY;
        }

        /// <summary>
        /// Called before connections are setup and bot is initialized
        /// </summary>
        /// <returns>A const int</returns>
        /// <seealso cref="MODULE_OKAY"/>
        /// <seealso cref="MODULE_ERROR"/>
        /// <seealso cref="MODULE_FATAL"/>
        public abstract int AddConfig();

        /// <summary>
        /// Called every 15 seconds after the bot is initialized. Use this for checks of a sort.
        /// </summary>
        /// <returns>A const int</returns>
        /// <seealso cref="MODULE_OKAY"/>
        /// <seealso cref="MODULE_ERROR"/>
        /// <seealso cref="MODULE_FATAL"/>
        public abstract int OnTick();

        /// <summary>
        /// Called when bot receives information on a connection
        /// </summary>
        /// <param name="sender">Boxed Connection object</param>
        /// <param name="e">Event args</param>
        public abstract void OnDataReceived(object sender, IRCReadEventArgs e);
    }
}
