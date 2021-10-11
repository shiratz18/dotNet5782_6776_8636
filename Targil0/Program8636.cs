using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome8636();
            Console.ReadKey();
        }

        static partial void Welcome6776();
        private static void Welcome8636()
        {
            Console.Write("Enter your name: ");
            string name;
            name = Console.ReadLine();
            Console.WriteLine($"{ name}, welcome to my first console application");
        }
    }
}
