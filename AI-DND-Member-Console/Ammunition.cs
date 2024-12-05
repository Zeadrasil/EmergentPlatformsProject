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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());
            sb.AppendLine("Dice:");
            foreach ((int, int) set in dice)
            {
                sb.AppendLine($"{set.Item1}d{set.Item2}");
            }
            sb.AppendLine($"Minimum Damage: {minDamage}");
            sb.AppendLine($"Damage Type: {damageType}");
            return sb.ToString();
        }
    }
}
