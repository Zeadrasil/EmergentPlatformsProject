using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_DND_Member_Console
{
    public class Weapon : Item
    {
        public string ammoType = "";
        public string damageType;
        public int minDamage = 0;
        public (int, int)[] damageDice;
        public CharacterSheet owner;

        public int rollDamage()
        {
            //Create storage variables
            Random rand = new Random();
            int damage = 0;

            //Go through each set of dice in weapon rolls (eg for something like 2d4 + 1d6)
            foreach ((int, int) set in damageDice)
            {
                for (int i = 0; i < set.Item1; i++)
                {
                    damage += rand.Next(set.Item2) + 1;
                }
            }
            //Apply minimum damage modifier, from like a +1 sword or something
            damage += minDamage;

            //If you do not fire ammunition, return the damage
            if (ammoType == "")
            {
                return damage;
            }
            //If you do, deal with it
            else if (owner.GetAmmunition(ammoType) != null)
            {
                Ammunition ammo = owner.GetAmmunition(ammoType);
                if (ammo.quantity > 0)
                {
                    //Go through all of the damage dice of the ammunition
                    foreach ((int, int) set in ammo.dice)
                    {
                        for (int i = 0; i < set.Item1; i++)
                        {
                            damage += rand.Next(set.Item2) + 1;
                        }
                    }
                    //apply the minimum damage of the ammo
                    return damage + ammo.minDamage;
                }
            }
            //If you are out of ammo you cannot use the weapon
            return 0;
        }

        //Turn into string for interactions with the AI
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());
            //Add ammo type if relevant
            if(ammoType != "")
            {
                sb.AppendLine($"\nAmmunition Type: {ammoType}");
            }
            else
            {
                sb.AppendLine("Ammunition Type: None");
            }
            //Add damage type
            sb.AppendLine($"Damage Type: {damageType}");

            //Begin adding dice
            sb.AppendLine("Dice:");

            //Loop through all dice
            foreach ((int, int) set in damageDice)
            {
                sb.AppendLine($"{set.Item1}d{set.Item2}");
            }
            //Add minimum damage
            sb.Append($"Minimum damage: {minDamage}");
            return sb.ToString();
        }
    }
}
