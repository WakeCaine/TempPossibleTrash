using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.AbstractFactory
{
    class UseAbstractConversation
    {
        private AbstractBye abstractBye;
        private AbstractHello abstractHello;

        public UseAbstractConversation(AbstractSay say)
        {
            abstractBye = say.SayBye();
            abstractHello = say.SayHello();
        }

        public void StartConversation()
        {
            abstractHello.Say(abstractBye);
        }
    }
}
