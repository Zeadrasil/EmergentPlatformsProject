using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_DND_Member_Console
{
    public class CharacterSheet
    {
        public string name;
        public string characterClass;
        public int characterLevel;

        int strength = 10;
        int dexterity = 10;
        int constitution = 10;
        int intelligence = 10;
        int wisdom = 10;
        int charisma = 10;

        int acrobatics = 0;
        int animalHandling = 0;
        int arcana = 0;
        int athletics = 0;
        int deception = 0;
        int history = 0;
        int intimidation = 0;
        int investigation = 0;
        int medicine = 0;
        int nature = 0;
        int perception = 0;
        int performance = 0;
        int persuasion = 0;
        int religion = 0;
        int sleightOfHand = 0;
        int stealth = 0;
        int survival = 0;

        int maxHealth;
        int currentHealth;
        int temporaryHealth;

        int armorClass;


        public List<Item> inventory = new List<Item>();
        public List<Ability> abilities = new List<Ability>();

        public int UseWeapon(string weapon)
        {
            foreach(Item item in inventory)
            {
                if (item.name == weapon)
                {
                    return ((Weapon)item).rollDamage();
                }
            }
            return -1;
        }

        public void useAbility(string ability, CharacterSheet target)
        {
            foreach(Ability checkingAbility in abilities)
            {
                if(checkingAbility.name == ability)
                {
                    if(checkingAbility.healing)
                    {
                        target.Heal(checkingAbility.GetHealing());
                    }
                    else if(checkingAbility.damage)
                    {
                        target.Damage(checkingAbility.GetDamage());
                    }
                    return;
                }
            }
        }

        public void Heal(int amount)
        {

        }
        public void Damage(int amount)
        {

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"Name: {name}\nClass: {characterClass}\nLevel: {characterLevel}\nStrength: {strength}\nDexterity: {dexterity}\nConstitution: {constitution}\nIntelligence: {intelligence}\nWisdom: {wisdom}\nCharisma: {charisma}\nAbilities:\n");
            foreach(Ability ability in abilities)
            {
                sb.AppendLine(ability.ToString());
            }
            sb.AppendLine("Inventory:");
            foreach(Item item in inventory)
            {
                sb.AppendLine(item.ToString());
            }
            string result = sb.ToString();
            return result.Remove(result.Length - 1);
        }
    }
}
