using System;
using System.Threading.Tasks;
using OllamaSharp;

namespace AI_DND_Member_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Will you be operating as the DM for this session?");
            //Testing.Run();
            string answer = Console.ReadLine();
            if (Testing.ProcessTrueFalseQuestion("Will you be acting as the DM?", answer) || Testing.ProcessTrueFalseQuestion("What role will you be playing?", "they are a Dungeon Master", answer))
            {
                RunAsDM();
            }
            else
            {
                RunAsPlayer();
            }
        }

        static void RunAsDM()
        {
            string answer;
            do
            {
                Console.WriteLine("Would you like to exit the application (DM)?");
                answer = Console.ReadLine();
            }
            while (!Testing.ProcessTrueFalseQuestion("Would you like to exit the application?", answer) && !Testing.ProcessTrueFalseQuestion("What would you like to do next?", "they would like to exit the application", answer));
        }
        static void RunAsPlayer()
        {
            string answer;
            do
            {
                Console.WriteLine("Would you like to exit the application (Player)?");
                answer = Console.ReadLine();
            }
            while (!Testing.ProcessTrueFalseQuestion("Would you like to exit the application?", answer) && !Testing.ProcessTrueFalseQuestion("What would you like to do next?", "they would like to exit the application", answer));
        }
    }
}
