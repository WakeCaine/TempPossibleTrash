using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.Prototype
{
    abstract class Clone
    {
        private String name;

        public Clone(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { 
                return name; 
            }
        }

        public abstract Clone CloneMe();
    }
}
