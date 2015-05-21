using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.AbstractFactory
{
    abstract class AbstractHello
    {
        public abstract void Say(AbstractBye b);
    }
}
