using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.Builder
{
    class Owner2 : BookStore
    {
        private Books books = new Books();

        public override void AddBook1()
        {
            books.Add("40 days arount the world");
        }

        public override void AddBook2()
        {
            books.Add("40 dni dookoła świata");
        }

        public override Books GetBooks()
        {
            return books;
        }
    }
}
