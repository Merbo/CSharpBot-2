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

        public static List<Type> GetClasses(Type baseType)
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(type => type.IsSubclassOf(baseType)).ToList();
        }

        public Core()
        {
            
        }

        public void SetupConfig()
        {
            Config = new Configuration();
            Config.RunInteractiveConfigWizard();
        }

        public void SetupIRCManager()
        {
            IrcManager = new IRCManager(Config);
        }

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
