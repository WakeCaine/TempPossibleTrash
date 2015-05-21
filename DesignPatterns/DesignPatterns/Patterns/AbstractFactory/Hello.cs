using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.AbstractFactory
{
    class Hello : AbstractHello
    {
        public override void Say(AbstractBye b)
        {
            Console.WriteLine("Hello this class: " + this.GetType().Name + " " + b.GetType().Name);
        }
    }
}
