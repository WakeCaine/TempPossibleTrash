using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.Builder
{
    class Owner1 : BookStore
    {
        private Books books = new Books();

        public override void AddBook1()
        {
            books.Add("Alicja w krainie czarów");
        }

        public override void AddBook2()
        {
            books.Add("Alice in wonderland");
        }

        public override Books GetBooks()
        {
            return books;
        }
    }
}
