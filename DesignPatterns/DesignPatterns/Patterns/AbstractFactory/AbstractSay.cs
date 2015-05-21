using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.AbstractFactory
{
    abstract class AbstractSay
    {
        public abstract AbstractHello SayHello();
        public abstract AbstractBye SayBye();
    }
}
