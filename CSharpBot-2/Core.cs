using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CSharpBot
{
    class Core
    {
        public Configuration Config;
        public IRCManager IrcManager;

        /// <summary>
        /// List of types, used for modules
        /// </summary>
        /// <param name="baseType">typeof(Module)</param>
        /// <returns></returns>
        public static List<Type> GetClasses(Type baseType)
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(type => type.IsSubclassOf(baseType)).ToList();
        }

        public Core()
        {   
        }

        /// <summary>
        /// Setup the config
        /// </summary>
        public void SetupConfig()
        {
            Config = new Configuration();
            Config.RunInteractiveConfigWizard();
        }

        /// <summary>
        /// Setup the IRC Manager
        /// </summary>
        public void SetupIRCManager()
        {
            IrcManager = new IRCManager(Config);
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="o">Any object</param>
        /// <param name="Level">LogLevel</param>
        /// <param name="WriteLine">Whether or not to finish the line or leave it open</param>
        public static void Log(object o, LogLevel Level, bool WriteLine = true)
        {
            string LevelPrefix = "          | ";
            Console.ResetColor();
            switch (Level)
            {
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    LevelPrefix = "[DEBUG]   | ";
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    LevelPrefix = "[INFO]    | ";
                    break;
                case LogLevel.Notice:
                    Console.ForegroundColor = ConsoleColor.White;
                    LevelPrefix = "[NOTICE]  | ";
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    LevelPrefix = "[WARNING] | ";
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    LevelPrefix = "[ERROR]   | ";
                    break;
                    //Begin custom values
                case LogLevel.Config:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    LevelPrefix = "[CONFIG]  | ";
                    break;
            }

            Console.Write(LevelPrefix);
            Console.Write(o);
            if (WriteLine)
                Console.Write(Environment.NewLine);

            Console.ResetColor();
        }

        /// <summary>
        /// LogLevel for Log
        /// </summary>
        public enum LogLevel
        {
            Debug,
            Info,
            Notice,
            Warning,
            Error,
            //Begin custom values
            Config,
        };
    }
}
