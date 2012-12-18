using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CSharpBot
{
    class Program
    {
        public static Core C;
        public static List<Module> Modules;
        static void Main(string[] args)
        {
            C = new Core();
            Modules = new List<Module>();
            foreach (Type T in Core.GetClasses(typeof(Module)))
            {
                if (T.IsClass)
                {
                    object boxedModule = Activator.CreateInstance(Type.GetType(T.FullName));
                    Module unboxedModule = (Module)boxedModule;
                    Modules.Add(unboxedModule);
                }
            }

            C.SetupConfig();
            foreach (Module M in Modules)
            {
                if (M.AddConfig() != Module.MODULE_OKAY)
                {
                    Core.Log("Module " + M.GetName() + " Threw an error!", Core.LogLevel.Error);
                }
            }
            C.SetupIRCManager();
            foreach (Module M in Modules)
            {
                if (M.SubscribeEvents() != Module.MODULE_OKAY)
                {
                    Core.Log("Module " + M.GetName() + " Threw an error!", Core.LogLevel.Error);
                }
            }
            C.IrcManager.SetupConnections();
            foreach (Module M in Modules)
            {
                if (M.SubscribeEventsMain() != Module.MODULE_OKAY)
                {
                    Core.Log("Module " + M.GetName() + " Threw an error!", Core.LogLevel.Error);
                }
            }
            foreach (Module M in Modules)
            {
                if (M.Run() != Module.MODULE_OKAY)
                {
                    Core.Log("Module " + M.GetName() + " Threw an error!", Core.LogLevel.Error);
                }
            }

            new Thread(() =>
            {
                while (true)
                {
                    foreach (Module M in Modules)
                    {
                        if (M.OnTick() != Module.MODULE_OKAY)
                        {
                            Core.Log("Module " + M.GetName() + " Threw an error!", Core.LogLevel.Error);
                        }
                    }
                    Thread.Sleep(15000);
                }
            }).Start();
        }
    }
}
