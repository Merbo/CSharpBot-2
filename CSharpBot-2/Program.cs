using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    class Program
    {
        public static Core C;
        static void Main(string[] args)
        {
            C = new Core();
            List<Type> TModules = Core.GetClasses(typeof(Module));
            List<Module> Modules = new List<Module>();
            foreach (Type T in TModules)
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
                if (M.SubscribeEventRead() != Module.MODULE_OKAY)
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
        }
    }
}
