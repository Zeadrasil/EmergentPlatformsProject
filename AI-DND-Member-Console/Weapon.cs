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
            Random rand = new Random();
            int damage = 0;
            foreach ((int, int) set in damageDice)
            {
                for (int i = 0; i < set.Item1; i++)
                {
                    damage += rand.Next(set.Item2) + 1;
                }
            }
            damage += minDamage;
            if (ammoType == null)
            {
                return damage;
            }
            else if(ammoType.quantity > 0)
            {
                foreach((int, int) set in ammoType.dice)
                {
                    for(int i = 0; i < set.Item1; i++)
                    {
                        damage += rand.Next(set.Item2) + 1;
                    }
                }
                return damage + ammoType.minDamage;
            }
            return 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());
            if(ammoType != null)
            {
                sb.AppendLine($"\nAmmunition Type: {ammoType}");
            }
            else
            {
                sb.AppendLine("Ammunition Type: None");
            }
            sb.AppendLine($"Damage Type: {damageType}");
            sb.AppendLine("Dice:");
            foreach ((int, int) set in damageDice)
            {
                sb.AppendLine($"{set.Item1}d{set.Item2}");
            }
            sb.Append($"Minimum damage: {minDamage}");
            return sb.ToString();
        }
    }
}
