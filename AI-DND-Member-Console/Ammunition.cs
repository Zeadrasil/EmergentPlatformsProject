using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_DND_Member_Console
{
    public class Ammunition : Item
    {
        public (int, int)[] dice;
        public int minDamage;
        public string damageType;

        //Turn into string for interacting with AI
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());
            //Start adding dice
            sb.AppendLine("Dice:");

            //Loop through all damage dice
            foreach ((int, int) set in dice)
            {
                sb.AppendLine($"{set.Item1}d{set.Item2}");
            }
            //Add minimum damage
            sb.AppendLine($"Minimum Damage: {minDamage}");

            //Add damage type
            sb.AppendLine($"Damage Type: {damageType}");
            return sb.ToString();
        }
    }
}
