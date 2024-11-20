using System;

namespace AI_DND_Member_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            do
            {
                Console.WriteLine(Console.ReadLine());
                Console.WriteLine("Press n to end");
            }
            while ((Console.ReadLine() != "n"));
        }
    }
}
