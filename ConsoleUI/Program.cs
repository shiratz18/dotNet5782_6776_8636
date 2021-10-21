using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            ShowMenu();
            Console.WriteLine("Please pick a choice:");
            Console.WriteLine("1. Add");
            Console.WriteLine("2. Update");
            Console.WriteLine("3. Status");
            Console.WriteLine("4. List display");
            Console.WriteLine("5. Exit");
            int choice;
            choice = int.Parse(Console.ReadLine());
            switch(choice)
            {
                

            }
        }
        private static void ShowMenu()
        {

        }
    }
}
