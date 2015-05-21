using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.AbstractFactory
{
    class AbstractConversation2 : AbstractSay
    {
        public override AbstractHello SayHello()
        {
            return new Hello();
        }

        public override AbstractBye SayBye()
        {
            return new Bye();
        }
    }
}
