using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.Builder
{
    abstract class BookStore
    {
        public abstract void AddBook1();
        public abstract void AddBook2();
        public abstract Books GetBooks();
    }
}
