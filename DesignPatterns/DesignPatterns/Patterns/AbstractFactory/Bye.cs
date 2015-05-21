using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.AbstractFactory
{
    class Bye : AbstractBye
    {
        public void Say()
        {
            Console.WriteLine("Bye this class " + this.GetType().Name);
        }
    }
}
