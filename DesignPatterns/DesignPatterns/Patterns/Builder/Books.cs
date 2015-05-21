using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Patterns.Builder
{
    class Books
    {
        ArrayList books = new ArrayList();
        public void Add(string detail){
            books.Add(detail);
        }

        public void ShowDetails()
        {
            Console.Write("Books: ");
            int i = 0;
            foreach(var book in books)
            {   
                ++i;
                Console.WriteLine(i + ": " + book);
            }
        }
    }
}
