using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.Prototype
{
    class CloneNumber2 : Clone
    {
        public CloneNumber2(string name)
            : base(name)
        {

        }

        public override Clone CloneMe()
        {
            return (Clone)this.MemberwiseClone();
        }
    }
}
