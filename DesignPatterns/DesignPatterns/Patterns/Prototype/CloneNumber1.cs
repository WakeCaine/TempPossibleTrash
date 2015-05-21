using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.Prototype
{
    class CloneNumber1 : Clone
    {
        public CloneNumber1(string name)
            : base(name)
        {

        }

        public override Clone CloneMe()
        {
            return (Clone)this.MemberwiseClone();
        }
    }
}
