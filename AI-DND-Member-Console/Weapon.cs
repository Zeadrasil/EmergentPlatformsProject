using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_DND_Member_Console
{
    public class Weapon : Item
    {
        public Ammunition ammoType = null;
        public string damageType;
        public int minDamage = 0;
        public (int, int)[] damageDice;

        public int rollDamage()
        {
            if(ammoType == null)
            {
                Random rand = new Random();
                int damage = 0;
                foreach((int, int) set in damageDice)
                {
                    for(int i = 0; i < set.Item1; i++)
                    {
                        damage += rand.Next(set.Item2) + 1 + minDamage;
                    }
                }
                return damage;
            }
            return -1;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());
            sb.AppendLine($"\nDamage Type: {damageType}\nDice:");
            foreach ((int, int) set in damageDice)
            {
                sb.AppendLine($"{set.Item1}d{set.Item2}");
            }
            sb.Append($"Minimum damage: {minDamage}");
            return sb.ToString();
        }
    }
}
