using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBot
{
    public abstract class Module
    {
        public Module()
        {
        }
        ~Module()
        {
        }
        public virtual void Run()
        {
        }
    }
}
