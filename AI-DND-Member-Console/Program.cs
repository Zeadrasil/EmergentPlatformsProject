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
                CharacterSheet sheet = new CharacterSheet();
                sheet.name = "Obama";
                sheet.characterLevel = 3;
                sheet.characterClass = "Warrior";
                Weapon weapon = new Weapon();
                weapon.damageDice = [(2, 6)];
                weapon.name = "Hammer";
                weapon.damageType = "Bludgeoning";
                sheet.inventory.Add(weapon);
                Ability ability = new Ability();
                ability.name = "Smash";
                ability.damage = true;
                ability.damageDice = [(3, 10)];
                sheet.abilities.Add(ability);
                Console.WriteLine(Testing.GetResponse($"Please summarize the following DnD character sheet in a readable format: {sheet.ToString()}"));
                Console.WriteLine("Use an ability:");
                Console.WriteLine(Testing.GetResponse($"Please use the ability \"{Console.ReadLine()}\" on the following character sheet: {sheet.ToString()}. If the character has an ability with a similar name, please respond with only the ability name, and if they do not respond with \"No Matching Ability\"."));
                Console.WriteLine("Use a weapon:");
                Console.WriteLine(Testing.GetResponse($"Please use the weapon \"{Console.ReadLine()}\" on the following character sheet: {sheet.ToString()}. If the character has a weapon with a similar name, please respond with only the weapon name, and if they do not respond with \"No Matching Weapon\"."));
                Console.WriteLine(Testing.GetResponse($"Please respond with the following character sheet except with the name of the \"Smash\" ability replaced with the name \"Hulk Smash\": {sheet.ToString()}"));
                Console.WriteLine(Testing.GetResponse($"Please generate a character sheet similar but not the same as the following at level 1: {sheet.ToString()}"));
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
