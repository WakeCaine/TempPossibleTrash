using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            DesignPatterns design1 = DesignPatterns.ObjectOfClass();
            design1.checkInstance = 15;
            DesignPatterns design2 = DesignPatterns.ObjectOfClass();
            Console.WriteLine("Pierwszy obiekt wartość: " + design1.checkInstance + " Drugi obiekt: " + design2.checkInstance);

            design1.ChooseTask();
            Console.ReadKey();
        }
    }
}
