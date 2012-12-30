using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;

namespace CSharpBot
{
    class cmd_HotCSharp : Module
    {
        private Tuple<Assembly, string> BuildAssembly(string code)
        {
            Microsoft.CSharp.CSharpCodeProvider provider = new CSharpCodeProvider();
            ICodeCompiler compiler = provider.CreateCompiler();
            CompilerParameters compilerparams = new CompilerParameters();
            compilerparams.GenerateExecutable = false;
            compilerparams.GenerateInMemory = true;
            compilerparams.CompilerOptions += "/unsafe";

            var assemblies = AppDomain.CurrentDomain
                            .GetAssemblies()
                            .Where(a => !a.IsDynamic)
                            .Select(a => a.Location);

            foreach (string s in assemblies.ToArray())
                compilerparams.ReferencedAssemblies.Add(s);
            CompilerResults results = compiler.CompileAssemblyFromSource(compilerparams, code);
            if (results.Errors.HasErrors)
            {
                StringBuilder errors = new StringBuilder("Compiler Errors\r\n");
                foreach (CompilerError error in results.Errors)
                {
                    errors.AppendFormat("Line {0},{1}\t: {2}\n", error.Line, error.Column, error.ErrorText);
                }
                return new Tuple<Assembly, string>(null, errors.ToString());
            }
            else
            {
                return new Tuple<Assembly, string>(results.CompiledAssembly, null);
            }
        }

        public object ExecuteCode(string code, string namespacename, string classname, string functionname, bool isstatic, params object[] args)
        {
            object returnval = null;
            Tuple<Assembly, string> builtasm = BuildAssembly(code);
            Assembly asm = builtasm.Item1;
            if (asm != null)
            {
                object instance = null;
                Type type = null;
                if (isstatic)
                {
                    type = asm.GetType(namespacename + "." + classname);
                }
                else
                {
                    instance = asm.CreateInstance(namespacename + "." + classname);
                    type = instance.GetType();
                }
                MethodInfo method = null;
                if (type != null)
                    method = type.GetMethod(functionname);
                if (method != null)
                    try
                    {
                        returnval = method.Invoke(instance, args);
                    }
                    catch (Exception ex)
                    {
                        returnval = ex.ToString();
                    }
                else
                    returnval = "ERROR: Method was null";
                return returnval;
            }
            else
                return builtasm.Item2;
        }

        public override string GetName()
        {
            return "cmd_HotCSharp";
        }

        public override string GetAuthor()
        {
            return "Merbo";
        }

        public override string GetDescription()
        {
            return "Allows for hot-running of C# code.";
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
            if (e.isAdmin &&
                e.Split[3].ToLower() == ":" + Program.C.Config.CommandPrefix.ToLower() + "c#")
            {
                Connection C = (Connection)sender;
                string code = string.Join(" ", e.Split, 4, e.Split.Length - 4);
                if (File.Exists(code))
                    code = File.ReadAllText(code);
                object ret = ExecuteCode(code, "CSharpBot", "Program", "Main", true, e.Nick + "!" + e.User + "@" + e.Host);
                if (ret == null)
                    ret = "\x03" + "4NULL";

                C.WriteLine("PRIVMSG " + e.Split[2] + " :" + e.Nick + ", return:");
                C.WriteLine("PRIVMSG " + e.Split[2] + " :" + ret.ToString().Replace("\n", " - ").Replace("\r", ""));
            }
        }

        public override void OnHelpReceived(object sender, IRCHelpEventArgs e)
        {
            if (e.Topic == "c#")
            {
                Connection C = (Connection)sender;
                C.WriteLine("PRIVMSG " + e.Target + " :" + e.Nick + ", usage:");
                C.WriteLine("PRIVMSG " + e.Target + " :" + Program.C.Config.CommandPrefix + "C# <code>");
                C.WriteLine("PRIVMSG " + e.Target + " :Where <code> is C# code to compile and execute.");
                C.WriteLine("PRIVMSG " + e.Target + " :Example:");
                C.WriteLine("PRIVMSG " + e.Target + " :" + Program.C.Config.CommandPrefix + "C# using System; namespace CSharpBot { class Program { public static string Main() { return \"Hello world!\"; } } }");
            }
        }
    }
}
