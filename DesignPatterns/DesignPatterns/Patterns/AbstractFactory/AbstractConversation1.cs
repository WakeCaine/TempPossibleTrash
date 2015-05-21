using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.AbstractFactory
{
    class AbstractConversation1 : AbstractSay
    {
        public override AbstractHello SayHello()
        {
            return new HelloCopy();
        }

        public override AbstractBye SayBye()
        {
            return new ByeCopy();
        }
    }
}
