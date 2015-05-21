using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.AbstractFactory
{
    class HelloCopy : AbstractHello
    {

        public override void Say(AbstractBye b)
        {
            Console.WriteLine("Hullo");

        }
    }
}
