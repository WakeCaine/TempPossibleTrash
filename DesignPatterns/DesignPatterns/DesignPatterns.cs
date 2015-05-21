using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesignPatterns.Patterns.AbstractFactory;
using DesignPatterns.Patterns.Builder;
using DesignPatterns.Patterns.Prototype;
using DesignPatterns.Patterns.Adapter;

namespace DesignPatterns
{
    //Singleton
    public class DesignPatterns
    {
        public int checkInstance;
        private static DesignPatterns objectOfClass;

        protected DesignPatterns(){ }

        public static DesignPatterns ObjectOfClass()
        {
            if (objectOfClass == null)
            {
                objectOfClass = new DesignPatterns();
            }
            return objectOfClass;
        }

        public void ChooseTask()
        {
            Console.WriteLine("Choose task to execute(1-4):");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Publisher publisher = new Publisher();
                        BookStore bookStore1 = new Owner1();
                        BookStore bookStore2 = new Owner2();

                        publisher.AddCollection(bookStore1);
                        Books books1 = bookStore1.GetBooks();
                        books1.ShowDetails();

                        publisher.AddCollection(bookStore2);
                        Books books2 = bookStore2.GetBooks();
                        books1.ShowDetails();
                        break;
                    }
                case "2":
                    {
                        CloneNumber1 clone1 = new CloneNumber1("Jahn");
                        CloneNumber1 cclone1 = (CloneNumber1)clone1.CloneMe();
                        Console.WriteLine("I was cloned: {0}", cclone1.Name);

                        CloneNumber2 clone2 = new CloneNumber2("Jahn Jahn");
                        CloneNumber2 cclone2 = (CloneNumber2)clone2.CloneMe();
                        Console.WriteLine("Iwas cloned: {0}", cclone2.Name);

                        break;
                    }
                case "3":
                    {
                        AbstractSay say1 = new AbstractConversation1();
                        UseAbstractConversation use1 = new UseAbstractConversation(say1);
                        use1.StartConversation();

                        AbstractSay say2 = new AbstractConversation2();
                        UseAbstractConversation use2 = new UseAbstractConversation(say2);
                        use2.StartConversation();
                        break;
                    }
                case "4":
                    {
                        Adapter adpt = new Adapter();
                        adpt.AddProduct();
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
