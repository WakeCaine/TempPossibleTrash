using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.Builder
{
    class Publisher
    {
        public void AddCollection(BookStore bookStore)
        {
            bookStore.AddBook1();
            bookStore.AddBook2();
        }
    }
}
