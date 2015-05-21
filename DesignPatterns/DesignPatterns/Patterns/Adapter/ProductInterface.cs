using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.Adapter
{
    class ProductInterface
    {
        public virtual void AddProduct()
        {
            Console.WriteLine("Add normal product.");
        }
    }
}
