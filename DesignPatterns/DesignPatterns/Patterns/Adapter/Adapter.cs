using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.Adapter
{
    class Adapter : ProductInterface
    {
        private RTV rtv = new RTV();

        public override void AddProduct()
        {
            rtv.AddElectronics();
        }
    }
}
