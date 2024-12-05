using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AI_DND_Member_Console
{
    public class Ability
    {
        public string name;
        public string description;
        public bool healing;
        public bool damage;

        public (int, int)[] healingDice;

        public (int, int)[] damageDice;

        //Get the amount that the ability heals
        public int GetHealing()
        {
            //Check if you can actually heal with this ability
            if (healing)
            {
                //Healing data storage
                Random rand = new Random();
                int healing = 0;

                //Go through each dice set for healing
                foreach ((int, int) set in healingDice)
                {
                    for (int i = 0; i < set.Item1; i++)
                    {
                        healing += rand.Next(set.Item2);
                    }
                }
                //Return the amount you healed with this usage
                return healing;
            }
            return 0;
        }

        //Get the damage a use of this ability would deal
        public int GetDamage()
        {
            //If the ability does not deal damage, don't
            if (damage)
            {
                //Damage data storage
                Random rand = new Random();
                int damage = 0;

                //Go through each set of dice to add their damage
                foreach ((int, int) set in damageDice)
                {
                    for (int i = 0; i < set.Item1; i++)
                    {
                        damage += rand.Next(set.Item2);
                    }
                }
                return damage;
            }
            return 0;
        }

        //Turn into a string for interacting with AI
       public string ToString()
        {
            StringBuilder sb = new StringBuilder($"Ability Name: {name}\nDescription: {description}\n");

            //If it is a healing ability, say so and also list all of the dice used to determine how much
            if (healing)
            {
                sb.AppendLine("Type: Healing\nDice:\n");
                foreach ((int, int) set in healingDice)
                {
                    sb.AppendLine($"{set.Item1}d{set.Item2}");
                }
            }
            //If it is a healing ability, say so and also list all of the dice used to determin how much
            else if (damage)
            {
                sb.AppendLine("Type: Damaging\nDice:\n");
                foreach ((int, int) set in damageDice)
                {
                    sb.AppendLine($"{set.Item1}d{set.Item2}");
                }
            }
            string result = sb.ToString();
            return result.Remove(result.Length - 1);
        }
    }
}
