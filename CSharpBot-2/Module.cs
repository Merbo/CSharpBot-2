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
        }
        public abstract int Run();
        public abstract int Init();
    }
}
