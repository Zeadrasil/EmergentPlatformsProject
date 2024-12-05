using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AI_DND_Member_Console
{
    public class CharacterSheet
    {
        public string name = "";
        public string race = "";
        public string characterClass = "";
        public int characterLevel;
        public string details = "";


        public int strength = 10;
        public int dexterity = 10;
        public int constitution = 10;
        public int intelligence = 10;
        public int wisdom = 10;
        public int charisma = 10;

        public int maxHealth;
        public int currentHealth;
        public int temporaryHealth;

        public int armorClass;


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
            currentHealth = Math.Clamp(currentHealth + amount, 0, 100);
        }
        public string Damage(int amount)
        {
            if(temporaryHealth > 0)
            {
                temporaryHealth -= amount;
                if(temporaryHealth < 0)
                {
                    currentHealth += temporaryHealth;
                    temporaryHealth = 0;
                }
            }
            else
            {
                currentHealth -= amount;
            }
            if(currentHealth <= 0)
            {
                currentHealth = 0;
            }
            return currentHealth.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"Name: {name}\nRace: {race}\nClass: {characterClass}\nLevel: {characterLevel}\nStrength: {strength}\nDexterity: {dexterity}\nConstitution: {constitution}\nIntelligence: {intelligence}\nWisdom: {wisdom}\nCharisma: {charisma}\nHealth: {currentHealth}\nMax Health: {maxHealth}\nTemporary Health: {temporaryHealth}\nDetails: {details}\nAbilities:\n");
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

        public static CharacterSheet FromString(string str)
        {
            CharacterSheet characterSheet = new CharacterSheet();
            string[] lines = str.Split("\n");
            int readMode = 0;
            Ability creatingAbility = null;
            Item creatingItem = null;
            Ammunition creatingAmmunition = null;
            Weapon creatingWeapon = null;
            Dictionary<string, Ammunition> createdAmmos = new Dictionary<string, Ammunition>();
            Dictionary<string, List<Weapon>> awaitingAmmos = new Dictionary<string, List<Weapon>>();
            string awaitingName = "";
            (int, int) diceHolder;
            foreach(string line in lines)
            {
                string modifiedLine;
                switch(readMode)
                {
                    case 0:
                        {
                            if (line.ToLower().Contains("name:"))
                            {
                                modifiedLine = line.Remove(0, 5).Trim();
                                characterSheet.name = modifiedLine;
                            }
                            if (line.ToLower().Contains("race:"))
                            {
                                modifiedLine = line.Remove(0, 5).Trim();
                                characterSheet.race = modifiedLine;
                            }
                            else if (line.ToLower().Contains("class:"))
                            {
                                modifiedLine = line.Remove(0, 6).Trim();
                                characterSheet.characterClass = modifiedLine;
                            }
                            else if (line.ToLower().Contains("level:"))
                            {
                                modifiedLine = line.Remove(0, 6).Trim();
                                characterSheet.characterLevel = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("strength:"))
                            {
                                modifiedLine = line.Remove(0, 9).Trim();
                                characterSheet.strength = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("dexterity:"))
                            {
                                modifiedLine = line.Remove(0, 10).Trim();
                                characterSheet.dexterity = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("constitution:"))
                            {
                                modifiedLine = line.Remove(0, 13).Trim();
                                characterSheet.constitution = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("intelligence:"))
                            {
                                modifiedLine = line.Remove(0, 13).Trim();
                                characterSheet.intelligence = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("wisdom:"))
                            {
                                modifiedLine = line.Remove(0, 7).Trim();
                                characterSheet.wisdom = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("charisma:"))
                            {
                                modifiedLine = line.Remove(0, 9).Trim();
                                characterSheet.charisma = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("health:"))
                            {
                                modifiedLine = line.Remove(0, 7).Trim();
                                characterSheet.currentHealth = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("max health:"))
                            {
                                modifiedLine = line.Remove(0, 11).Trim();
                                characterSheet.maxHealth = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("maximum health:"))
                            {
                                modifiedLine = line.Remove(0, 15).Trim();
                                characterSheet.maxHealth = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("temp health:"))
                            {
                                modifiedLine = line.Remove(0, 12).Trim();
                                characterSheet.temporaryHealth = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("temporary health:"))
                            {
                                modifiedLine = line.Remove(0, 17).Trim();
                                characterSheet.strength = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("hitpoints:"))
                            {
                                modifiedLine = line.Remove(0, 10).Trim();
                                characterSheet.currentHealth = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("max hitpoints:"))
                            {
                                modifiedLine = line.Remove(0, 14).Trim();
                                characterSheet.maxHealth = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("maximum hitpoints:"))
                            {
                                modifiedLine = line.Remove(0, 19).Trim();
                                characterSheet.maxHealth = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("temp hitpoints:"))
                            {
                                modifiedLine = line.Remove(0, 16).Trim();
                                characterSheet.temporaryHealth = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("temporary hitpoints:"))
                            {
                                modifiedLine = line.Remove(0, 21).Trim();
                                characterSheet.strength = int.Parse(modifiedLine);
                            }
                            else if (line.ToLower().Contains("details:"))
                            {
                                modifiedLine = line.Remove(0, 8).Trim();
                                characterSheet.details = modifiedLine;
                            }
                            else if (line.ToLower().Contains("abilities:"))
                            {
                                readMode = 1;
                            }
                            else if (line.ToLower().Contains("inventory:"))
                            {
                                readMode = 3;
                            }
                            break;
                        }
                    case 1:
                        {
                            if(line.ToLower().Contains("ability name:"))
                            {
                                creatingAbility = new Ability();
                                creatingAbility.name = line.Remove(0, 13).Trim();
                            }
                            else if(line.ToLower().Contains("type:"))
                            {
                                modifiedLine = line.ToLower().Trim();
                                creatingAbility.healing = modifiedLine.ToLower() == "healing";
                                creatingAbility.damage = modifiedLine.ToLower() == "damaging";
                            }
                            else if(line.ToLower().Contains("dice:"))
                            {
                                readMode = 2;
                            }
                            else if(line.ToLower().Contains("inventory:"))
                            {
                                readMode = 3;
                            }
                            break;
                        }
                    case 2:
                        {
                            if(line.ToLower().Contains("ability name:"))
                            {
                                creatingAbility = new Ability();
                                creatingAbility.name = line.Remove(0, 13).Trim();
                                readMode = 1;
                            }
                            else if(line.ToLower().Contains("inventory:"))
                            {
                                readMode = 3;
                            }
                            else
                            {
                                try
                                {
                                    diceHolder = (0, 0);
                                    foreach(string s in line.Split('d'))
                                    {
                                        if(diceHolder.Item1 == 0)
                                        {
                                            diceHolder.Item1 = int.Parse(s);
                                        }
                                        else
                                        {
                                            diceHolder.Item2 = int.Parse(s);
                                        }
                                    }
                                    if(creatingAbility.healing)
                                    {
                                        List<(int, int)> allDiceHolder = new List<(int, int)>(creatingAbility.healingDice);
                                        allDiceHolder.Add(diceHolder);
                                        creatingAbility.healingDice = allDiceHolder.ToArray();
                                    }
                                    if(creatingAbility.damage)
                                    {
                                        List<(int, int)> allDiceHolder = new List<(int, int)>(creatingAbility.healingDice);
                                        allDiceHolder.Add(diceHolder);
                                        creatingAbility.healingDice = allDiceHolder.ToArray();
                                    }

                                }
                                catch
                                {
                                    readMode = 1;
                                }
                            }
                            break;
                        }
                    case 3:
                        {
                            if (line.ToLower().Contains("item name:"))
                            {
                                if(creatingItem != null)
                                {
                                    characterSheet.inventory.Add(creatingItem);
                                }
                                modifiedLine = line.Remove(0, 10).Trim();
                                creatingItem = new Item();
                                creatingItem.name = modifiedLine;
                                awaitingName = "";
                            }
                            else if(line.ToLower().Contains("quantity:"))
                            {
                                modifiedLine = line.Remove(0, 9).Trim();
                                creatingItem.quantity = int.Parse(modifiedLine);
                            }
                            else if(line.ToLower().Contains("description:"))
                            {
                                modifiedLine = line.Remove(0, 12).Trim();
                                creatingItem.description = modifiedLine;
                            }
                            else if(line.ToLower().Contains("ammunition type:"))
                            {
                                readMode = 4;
                                creatingWeapon = new Weapon();
                                creatingWeapon.name = creatingItem.name;
                                creatingWeapon.quantity = creatingItem.quantity;
                                creatingWeapon.description = creatingItem.description;
                                modifiedLine = line.Remove(0, 16).Trim().ToLower();
                                if(createdAmmos.TryGetValue(modifiedLine, out Ammunition ammo))
                                {
                                    creatingWeapon.ammoType = ammo;
                                    awaitingName = "";
                                }
                                else
                                {
                                    awaitingName = modifiedLine;
                                    if (awaitingAmmos.TryGetValue(modifiedLine, out List<Weapon> weaponsAlreadyPresent))
                                    {
                                        weaponsAlreadyPresent.Add(creatingWeapon);
                                        awaitingAmmos[modifiedLine] = weaponsAlreadyPresent;
                                    }
                                    else
                                    {
                                        awaitingAmmos.Add(modifiedLine, new List<Weapon> { creatingWeapon });
                                    }
                                }
                                creatingItem = creatingWeapon;
                            }
                            else if(line.ToLower().Contains("damage type:"))
                            {
                                readMode = 4;
                                creatingWeapon = new Weapon();
                                creatingWeapon.name = creatingItem.name;
                                creatingWeapon.quantity = creatingItem.quantity;
                                creatingWeapon.description = creatingItem.description;
                                modifiedLine = line.Remove(0, 12);
                                creatingWeapon.damageType = modifiedLine;
                                creatingItem = creatingWeapon;
                            }
                            else if(line.ToLower().Contains("dice:"))
                            {
                                readMode = 6;
                                creatingAmmunition = new Ammunition();
                                creatingAmmunition.name = creatingItem.name;
                                creatingAmmunition.quantity = creatingItem.quantity;
                                creatingAmmunition.description = creatingItem.description;
                                if(awaitingAmmos.TryGetValue(creatingAmmunition.name.ToLower(), out List<Weapon> weapons))
                                {
                                    foreach(Weapon weapon in weapons)
                                    {
                                        weapon.ammoType = creatingAmmunition;
                                    }
                                }
                                createdAmmos.Add(creatingAmmunition.name.ToLower(), creatingAmmunition);
                                creatingItem = creatingAmmunition;
                            }
                            break;
                        }
                    case 4:
                        {
                            if (line.ToLower().Contains("damage type:"))
                            {
                                modifiedLine = line.Remove(0, 12);
                                creatingWeapon.damageType = modifiedLine;
                            }
                            else if(line.ToLower().Contains("dice:"))
                            {
                                readMode = 5;
                            }
                            break;
                        }
                    case 5:
                        {
                            if (line.ToLower().Contains("minimum damage:"))
                            {
                                readMode = 3;
                                modifiedLine = line.Remove(0, 15);
                                creatingWeapon.minDamage = int.Parse(modifiedLine);
                            }
                            else if(!string.IsNullOrWhiteSpace(line))
                            {
                                try
                                {
                                    diceHolder = (0, 0);
                                    foreach (string s in line.Split('d'))
                                    {
                                        if (diceHolder.Item1 == 0)
                                        {
                                            diceHolder.Item1 = int.Parse(s);
                                        }
                                        else
                                        {
                                            diceHolder.Item2 = int.Parse(s);
                                        }
                                    }
                                    List<(int, int)> allDiceHolder = new List<(int, int)>(creatingWeapon.damageDice);
                                    allDiceHolder.Add(diceHolder);
                                    creatingWeapon.damageDice = allDiceHolder.ToArray();
                                }
                                catch
                                {

                                }
                            }
                            break;
                        }
                    case 6:
                        {
                            if(line.ToLower().Contains("item name:"))
                            {
                                readMode = 3; 
                                if (creatingItem != null)
                                {
                                    characterSheet.inventory.Add(creatingItem);
                                }
                                modifiedLine = line.Remove(0, 10).Trim();
                                creatingItem = new Item();
                                creatingItem.name = modifiedLine;
                                awaitingName = "";
                            }
                            else if(line.ToLower().Contains("minimum damage:"))
                            {
                                modifiedLine = line.Remove(0, 15);
                                creatingAmmunition.minDamage = int.Parse(modifiedLine);
                            }
                            else if(line.ToLower().Contains("damage type:"))
                            {
                                modifiedLine = line.Remove(0, 12);
                                creatingAmmunition.damageType = modifiedLine;
                            }
                            else if(!string.IsNullOrWhiteSpace(line))
                            {
                                diceHolder = (0, 0);
                                foreach (string s in line.Split('d'))
                                {
                                    if (diceHolder.Item1 == 0)
                                    {
                                        diceHolder.Item1 = int.Parse(s);
                                    }
                                    else
                                    {
                                        diceHolder.Item2 = int.Parse(s);
                                    }
                                }
                                List<(int, int)> allDiceHolder = new List<(int, int)>(creatingAmmunition.dice);
                                allDiceHolder.Add(diceHolder);
                                creatingAmmunition.dice = allDiceHolder.ToArray();
                            }
                            creatingItem = creatingAmmunition;
                            break;
                        }
                }
                
            }
            return characterSheet;
        }
    }
}
