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
        public bool healing;
        public bool damage;

        public (int, int)[] healingDice;

        public (int, int)[] damageDice;

        public int GetHealing()
        {
            if (healing)
            {
                Random rand = new Random();
                int healing = 0;
                foreach ((int, int) set in healingDice)
                {
                    for (int i = 0; i < set.Item1; i++)
                    {
                        healing += rand.Next(set.Item2);
                    }
                }
                return healing;
            }
            return 0;
        }

        public int GetDamage()
        {
            if (damage)
            {
                Random rand = new Random();
                int damage = 0;
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

       public string ToString()
        {
            StringBuilder sb = new StringBuilder($"Ability Name: {name}\n");
            if (healing)
            {
                sb.AppendLine("Type: Healing\nDice:\n");
                foreach ((int, int) set in healingDice)
                {
                    sb.AppendLine($"{set.Item1}d{set.Item2}");
                }
            }
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
