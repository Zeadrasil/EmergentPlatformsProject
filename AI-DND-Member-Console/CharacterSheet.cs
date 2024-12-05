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

        //Find and use a weapon
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

        //Find and use an ability
        public void useAbility(string ability, CharacterSheet target)
        {
            foreach(Ability checkingAbility in abilities)
            {
                if(checkingAbility.name == ability)
                {
                    //Apply healing
                    if(checkingAbility.healing)
                    {
                        target.Heal(checkingAbility.GetHealing());
                    }
                    //Apply damage
                    else if(checkingAbility.damage)
                    {
                        target.Damage(checkingAbility.GetDamage());
                    }
                    return;
                }
            }
        }

        //Heal yourselg by given amount
        public void Heal(int amount)
        {
            currentHealth = Math.Clamp(currentHealth + amount, 0, 100);
        }

        //Damage yourself by given amount
        public int Damage(int amount)
        {
            //Use temporary health first
            if(temporaryHealth > 0)
            {
                temporaryHealth -= amount;

                //If temporary health is insufficient, use normal
                if(temporaryHealth < 0)
                {
                    currentHealth += temporaryHealth;
                    temporaryHealth = 0;
                }
            }
            //If no temporary health go straight to normal
            else
            {
                currentHealth -= amount;
            }
            //If out of health
            if(currentHealth <= 0)
            {
                currentHealth = 0;
            }
            return currentHealth;
        }

        //Get sheet as string for use with the AI
        public override string ToString()
        {
            //Get the basic sheet data
            StringBuilder sb = new StringBuilder($"Name: {name}\nRace: {race}\nClass: {characterClass}\nLevel: {characterLevel}\nStrength: {strength}\nDexterity: {dexterity}\nConstitution: {constitution}\nIntelligence: {intelligence}\nWisdom: {wisdom}\nCharisma: {charisma}\nHealth: {currentHealth}\nMax Health: {maxHealth}\nTemporary Health: {temporaryHealth}\nDetails: {details}\nAbilities:\n");
            
            //Go through and add all abilities
            foreach(Ability ability in abilities)
            {
                sb.AppendLine(ability.ToString());
            }
            //Go through inventory and add all of the items
            sb.AppendLine("Inventory:");
            foreach(Item item in inventory)
            {
                sb.AppendLine(item.ToString());
            }
            string result = sb.ToString();
            return result.Remove(result.Length - 1);
        }

        public Ammunition GetAmmunition(string name)
        {
            foreach(Item item in inventory)
            {
                if(item.name.ToLower().Contains(name.ToLower()))
                {
                    return (Ammunition)item;
                }
            }
            return null;
        }

        //Deprecated, but kept for reference later
        //public static CharacterSheet FromString(string str)
        //{
        //    CharacterSheet characterSheet = new CharacterSheet();
        //    string[] lines = str.Split("\n");
        //    int readMode = 0;
        //    Ability creatingAbility = null;
        //    Item creatingItem = null;
        //    Ammunition creatingAmmunition = null;
        //    Weapon creatingWeapon = null;
        //    Dictionary<string, Ammunition> createdAmmos = new Dictionary<string, Ammunition>();
        //    Dictionary<string, List<Weapon>> awaitingAmmos = new Dictionary<string, List<Weapon>>();
        //    string awaitingName = "";
        //    (int, int) diceHolder;
        //    foreach(string line in lines)
        //    {
        //        string modifiedLine;
        //        switch(readMode)
        //        {
        //            case 0:
        //                {
        //                    if (line.ToLower().Contains("name:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 5).Trim();
        //                        characterSheet.name = modifiedLine;
        //                    }
        //                    if (line.ToLower().Contains("race:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 5).Trim();
        //                        characterSheet.race = modifiedLine;
        //                    }
        //                    else if (line.ToLower().Contains("class:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 6).Trim();
        //                        characterSheet.characterClass = modifiedLine;
        //                    }
        //                    else if (line.ToLower().Contains("level:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 6).Trim();
        //                        characterSheet.characterLevel = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("strength:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 9).Trim();
        //                        characterSheet.strength = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("dexterity:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 10).Trim();
        //                        characterSheet.dexterity = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("constitution:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 13).Trim();
        //                        characterSheet.constitution = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("intelligence:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 13).Trim();
        //                        characterSheet.intelligence = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("wisdom:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 7).Trim();
        //                        characterSheet.wisdom = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("charisma:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 9).Trim();
        //                        characterSheet.charisma = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("health:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 7).Trim();
        //                        characterSheet.currentHealth = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("max health:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 11).Trim();
        //                        characterSheet.maxHealth = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("maximum health:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 15).Trim();
        //                        characterSheet.maxHealth = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("temp health:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 12).Trim();
        //                        characterSheet.temporaryHealth = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("temporary health:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 17).Trim();
        //                        characterSheet.strength = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("hitpoints:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 10).Trim();
        //                        characterSheet.currentHealth = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("max hitpoints:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 14).Trim();
        //                        characterSheet.maxHealth = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("maximum hitpoints:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 19).Trim();
        //                        characterSheet.maxHealth = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("temp hitpoints:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 16).Trim();
        //                        characterSheet.temporaryHealth = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("temporary hitpoints:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 21).Trim();
        //                        characterSheet.strength = int.Parse(modifiedLine);
        //                    }
        //                    else if (line.ToLower().Contains("details:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 8).Trim();
        //                        characterSheet.details = modifiedLine;
        //                    }
        //                    else if (line.ToLower().Contains("abilities:"))
        //                    {
        //                        readMode = 1;
        //                    }
        //                    else if (line.ToLower().Contains("inventory:"))
        //                    {
        //                        readMode = 3;
        //                    }
        //                    break;
        //                }
        //            case 1:
        //                {
        //                    if(line.ToLower().Contains("ability name:"))
        //                    {
        //                        creatingAbility = new Ability();
        //                        creatingAbility.name = line.Remove(0, 13).Trim();
        //                    }
        //                    else if(line.ToLower().Contains("type:"))
        //                    {
        //                        modifiedLine = line.ToLower().Trim();
        //                        creatingAbility.healing = modifiedLine.ToLower() == "healing";
        //                        creatingAbility.damage = modifiedLine.ToLower() == "damaging";
        //                    }
        //                    else if(line.ToLower().Contains("dice:"))
        //                    {
        //                        readMode = 2;
        //                    }
        //                    else if(line.ToLower().Contains("inventory:"))
        //                    {
        //                        readMode = 3;
        //                    }
        //                    break;
        //                }
        //            case 2:
        //                {
        //                    if(line.ToLower().Contains("ability name:"))
        //                    {
        //                        creatingAbility = new Ability();
        //                        creatingAbility.name = line.Remove(0, 13).Trim();
        //                        readMode = 1;
        //                    }
        //                    else if(line.ToLower().Contains("inventory:"))
        //                    {
        //                        readMode = 3;
        //                    }
        //                    else
        //                    {
        //                        try
        //                        {
        //                            diceHolder = (0, 0);
        //                            foreach(string s in line.Split('d'))
        //                            {
        //                                if(diceHolder.Item1 == 0)
        //                                {
        //                                    diceHolder.Item1 = int.Parse(s);
        //                                }
        //                                else
        //                                {
        //                                    diceHolder.Item2 = int.Parse(s);
        //                                }
        //                            }
        //                            if(creatingAbility.healing)
        //                            {
        //                                List<(int, int)> allDiceHolder = new List<(int, int)>(creatingAbility.healingDice);
        //                                allDiceHolder.Add(diceHolder);
        //                                creatingAbility.healingDice = allDiceHolder.ToArray();
        //                            }
        //                            if(creatingAbility.damage)
        //                            {
        //                                List<(int, int)> allDiceHolder = new List<(int, int)>(creatingAbility.healingDice);
        //                                allDiceHolder.Add(diceHolder);
        //                                creatingAbility.healingDice = allDiceHolder.ToArray();
        //                            }

        //                        }
        //                        catch
        //                        {
        //                            readMode = 1;
        //                        }
        //                    }
        //                    break;
        //                }
        //            case 3:
        //                {
        //                    if (line.ToLower().Contains("item name:"))
        //                    {
        //                        if(creatingItem != null)
        //                        {
        //                            characterSheet.inventory.Add(creatingItem);
        //                        }
        //                        modifiedLine = line.Remove(0, 10).Trim();
        //                        creatingItem = new Item();
        //                        creatingItem.name = modifiedLine;
        //                        awaitingName = "";
        //                    }
        //                    else if(line.ToLower().Contains("quantity:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 9).Trim();
        //                        creatingItem.quantity = int.Parse(modifiedLine);
        //                    }
        //                    else if(line.ToLower().Contains("description:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 12).Trim();
        //                        creatingItem.description = modifiedLine;
        //                    }
        //                    else if(line.ToLower().Contains("ammunition type:"))
        //                    {
        //                        readMode = 4;
        //                        creatingWeapon = new Weapon();
        //                        creatingWeapon.name = creatingItem.name;
        //                        creatingWeapon.quantity = creatingItem.quantity;
        //                        creatingWeapon.description = creatingItem.description;
        //                        modifiedLine = line.Remove(0, 16).Trim().ToLower();
        //                        if(createdAmmos.TryGetValue(modifiedLine, out Ammunition ammo))
        //                        {
        //                            creatingWeapon.ammoType = ammo;
        //                            awaitingName = "";
        //                        }
        //                        else
        //                        {
        //                            awaitingName = modifiedLine;
        //                            if (awaitingAmmos.TryGetValue(modifiedLine, out List<Weapon> weaponsAlreadyPresent))
        //                            {
        //                                weaponsAlreadyPresent.Add(creatingWeapon);
        //                                awaitingAmmos[modifiedLine] = weaponsAlreadyPresent;
        //                            }
        //                            else
        //                            {
        //                                awaitingAmmos.Add(modifiedLine, new List<Weapon> { creatingWeapon });
        //                            }
        //                        }
        //                        creatingItem = creatingWeapon;
        //                    }
        //                    else if(line.ToLower().Contains("damage type:"))
        //                    {
        //                        readMode = 4;
        //                        creatingWeapon = new Weapon();
        //                        creatingWeapon.name = creatingItem.name;
        //                        creatingWeapon.quantity = creatingItem.quantity;
        //                        creatingWeapon.description = creatingItem.description;
        //                        modifiedLine = line.Remove(0, 12);
        //                        creatingWeapon.damageType = modifiedLine;
        //                        creatingItem = creatingWeapon;
        //                    }
        //                    else if(line.ToLower().Contains("dice:"))
        //                    {
        //                        readMode = 6;
        //                        creatingAmmunition = new Ammunition();
        //                        creatingAmmunition.name = creatingItem.name;
        //                        creatingAmmunition.quantity = creatingItem.quantity;
        //                        creatingAmmunition.description = creatingItem.description;
        //                        if(awaitingAmmos.TryGetValue(creatingAmmunition.name.ToLower(), out List<Weapon> weapons))
        //                        {
        //                            foreach(Weapon weapon in weapons)
        //                            {
        //                                weapon.ammoType = creatingAmmunition;
        //                            }
        //                        }
        //                        createdAmmos.Add(creatingAmmunition.name.ToLower(), creatingAmmunition);
        //                        creatingItem = creatingAmmunition;
        //                    }
        //                    break;
        //                }
        //            case 4:
        //                {
        //                    if (line.ToLower().Contains("damage type:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 12);
        //                        creatingWeapon.damageType = modifiedLine;
        //                    }
        //                    else if(line.ToLower().Contains("dice:"))
        //                    {
        //                        readMode = 5;
        //                    }
        //                    break;
        //                }
        //            case 5:
        //                {
        //                    if (line.ToLower().Contains("minimum damage:"))
        //                    {
        //                        readMode = 3;
        //                        modifiedLine = line.Remove(0, 15);
        //                        creatingWeapon.minDamage = int.Parse(modifiedLine);
        //                    }
        //                    else if(!string.IsNullOrWhiteSpace(line))
        //                    {
        //                        try
        //                        {
        //                            diceHolder = (0, 0);
        //                            foreach (string s in line.Split('d'))
        //                            {
        //                                if (diceHolder.Item1 == 0)
        //                                {
        //                                    diceHolder.Item1 = int.Parse(s);
        //                                }
        //                                else
        //                                {
        //                                    diceHolder.Item2 = int.Parse(s);
        //                                }
        //                            }
        //                            List<(int, int)> allDiceHolder = new List<(int, int)>(creatingWeapon.damageDice);
        //                            allDiceHolder.Add(diceHolder);
        //                            creatingWeapon.damageDice = allDiceHolder.ToArray();
        //                        }
        //                        catch
        //                        {

        //                        }
        //                    }
        //                    break;
        //                }
        //            case 6:
        //                {
        //                    if(line.ToLower().Contains("item name:"))
        //                    {
        //                        readMode = 3; 
        //                        if (creatingItem != null)
        //                        {
        //                            characterSheet.inventory.Add(creatingItem);
        //                        }
        //                        modifiedLine = line.Remove(0, 10).Trim();
        //                        creatingItem = new Item();
        //                        creatingItem.name = modifiedLine;
        //                        awaitingName = "";
        //                    }
        //                    else if(line.ToLower().Contains("minimum damage:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 15);
        //                        creatingAmmunition.minDamage = int.Parse(modifiedLine);
        //                    }
        //                    else if(line.ToLower().Contains("damage type:"))
        //                    {
        //                        modifiedLine = line.Remove(0, 12);
        //                        creatingAmmunition.damageType = modifiedLine;
        //                    }
        //                    else if(!string.IsNullOrWhiteSpace(line))
        //                    {
        //                        diceHolder = (0, 0);
        //                        foreach (string s in line.Split('d'))
        //                        {
        //                            if (diceHolder.Item1 == 0)
        //                            {
        //                                diceHolder.Item1 = int.Parse(s);
        //                            }
        //                            else
        //                            {
        //                                diceHolder.Item2 = int.Parse(s);
        //                            }
        //                        }
        //                        List<(int, int)> allDiceHolder = new List<(int, int)>(creatingAmmunition.dice);
        //                        allDiceHolder.Add(diceHolder);
        //                        creatingAmmunition.dice = allDiceHolder.ToArray();
        //                    }
        //                    creatingItem = creatingAmmunition;
        //                    break;
        //                }
        //        }
                
        //    }
        //    return characterSheet;
        //}

        public static CharacterSheet FromTemplate(int level)
        {
            CharacterSheet characterSheet = new CharacterSheet();
            characterSheet.characterClass = SetClass();
            //characterSheet.characterClass = Testing.GetResponse("Please come up with a class for a DnD character. Do not include anything in your response but the class of the new character.");
            characterSheet.race = Testing.GetResponse($"You have started creating a DnD character in the class {characterSheet.characterClass}. Please respond with the race that this character will be. Do not include anything in your response but the name of the race of the new character.");
            characterSheet.name = Testing.GetResponse($"You have started creating a DnD character that is a {characterSheet.race} in the class {characterSheet.characterClass}. Please respond with what their name is. Do not include anything in your response but the name of the new character.");
            //characterSheet.name = Testing.GetResponse("Please come up with a name for a DnD character. Do not include anything in your response but the name of the new character.");
            //characterSheet.race = Testing.GetResponse($"You have started creating a DnD character named {characterSheet.name}. Please respond with the race that this character will be. Do not include anything in your response but the name of the race of the new character.");
            //characterSheet.characterClass = Testing.GetResponse($"You have started creating a DnD character that is a {characterSheet.race} named {characterSheet.name}. Please respond with what class they are. Do not include anything in your response but the name of the new character's class.");
            characterSheet.characterLevel = level;
            characterSheet.strength = int.Parse(Testing.GetResponse($"You have started creating a DnD character that is a level {level} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name}. Please respond with the strength value of the player, including nothing but the number of the stat in your response"));
            characterSheet.dexterity = int.Parse(Testing.GetResponse($"You have started creating a DnD character that is a level {level} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name}. Please respond with the dexterity value of the player, including nothing but the number of the stat in your response"));
            characterSheet.constitution = int.Parse(Testing.GetResponse($"You have started creating a DnD character that is a level {level} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name}. Please respond with the constitution value of the player, including nothing but the number of the stat in your response"));
            characterSheet.intelligence = int.Parse(Testing.GetResponse($"You have started creating a DnD character that is a level {level} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name}. Please respond with the intelligence value of the player, including nothing but the number of the stat in your response"));
            characterSheet.wisdom = int.Parse(Testing.GetResponse($"You have started creating a DnD character that is a level {level} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name}. Please respond with the wisdom value of the player, including nothing but the number of the stat in your response"));
            characterSheet.charisma = int.Parse(Testing.GetResponse($"You have started creating a DnD character that is a level {level} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name}. Please respond with the charisma value of the player, including nothing but the number of the stat in your response"));
            //characterSheet.maxHealth = int.Parse(Testing.GetResponse($"You have started creating a DnD character that is a level {level} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name} and the stats of Strength {characterSheet.strength}, Dexterity {characterSheet.dexterity}, Constitution {characterSheet.constitution}, Intelligence {characterSheet.intelligence}, Wisdom {characterSheet.wisdom}, Charisma {characterSheet.charisma}. What is the max health of this character? Respond with only the final number and nothing else."));
            characterSheet.maxHealth = SetHealth(level, characterSheet.characterClass, characterSheet.constitution);
            //characterSheet.details = Testing.GetResponse($"You have started creating a DnD character that is a level {level} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name} and the stats of Strength {characterSheet.strength}, Dexterity {characterSheet.dexterity}, Constitution {characterSheet.constitution}, Intelligence {characterSheet.intelligence}, Wisdom {characterSheet.wisdom}, Charisma {characterSheet.charisma}, and the max health of {characterSheet.maxHealth}. Please respond with the description of the character, including nothing in your response except for the character description.");
            characterSheet.details = Testing.GetResponse($"You have started creating a DnD character that is a level {level} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name} and the stats of Strength {characterSheet.strength}, Dexterity {characterSheet.dexterity}, Constitution {characterSheet.constitution}, Intelligence {characterSheet.intelligence}, Wisdom {characterSheet.wisdom}, Charisma {characterSheet.charisma}, and the max health of {characterSheet.maxHealth}. Please respond with the description of the character, including nothing in your response except for the character description. Attempt to keep your response to 100 words or less, and attempt to describe things about how the character acts and looks.");
            characterSheet.currentHealth = characterSheet.maxHealth;
            int count = int.Parse(Testing.GetResponse($"You have started creating a DnD character that is a level {level} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name} and the stats of Strength {characterSheet.strength}, Dexterity {characterSheet.dexterity}, Constitution {characterSheet.constitution}, Intelligence {characterSheet.intelligence}, Wisdom {characterSheet.wisdom}, Charisma {characterSheet.charisma}, and the max health of {characterSheet.maxHealth}, with the character details of \"{characterSheet.details}\". Please respond with how many abilities the character should have, including nothing in your response except for the number of abilities."));
            for(int i = 0; i < count; i++)
            {
                Ability ability = new Ability();
                StringBuilder sb = new StringBuilder($"You have started creating a DnD character that is a level {level} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name}. Please come up with a new ability");
                if (characterSheet.abilities.Count > 0)
                {
                    sb.Append(" that is not ");
                    for (int j = 0; j < characterSheet.abilities.Count - 1; j++)
                    {
                        sb.Append($"{characterSheet.abilities[j].name}, ");
                    }
                    if (characterSheet.abilities.Count == 1)
                    {
                        sb.Append($"{characterSheet.abilities[0].name}.");
                    }
                    else
                    {
                        sb.Append($"or {characterSheet.abilities[characterSheet.abilities.Count - 1].name}.");
                    }
                }
                else
                {
                    sb.Append(".");
                }
                sb.Append("Include nothing in your response but the name of the ability.");
                ability.name = Testing.GetResponse(sb.ToString());
                ability.description = Testing.GetResponse($"You have started crating an ability named {ability.name}. Please come up with a description in it, including nothing in your response except for the description. Attempt to keep this description 20 words long or shorter. Your goal should be to first describe what the ability does and if it is limited use, how many uses it has. Feel free to include a basic description of how it looks.");
                ability.healing = bool.Parse(Testing.GetResponse($"You have started creating an ability named {ability.name} and the description of \"{ability.description}\". Please respond with whether the ability results in a target being healed, limiting your response to \"true\" or \"false\" depending on whether it does or not. Do not include anything else in your response."));
                ability.damage = bool.Parse(Testing.GetResponse($"You have started creating an ability named {ability.name} and the description of \"{ability.description}\". Please respond with whether the ability results in a target being damaged, limiting your response to \"true\" or \"false\" depending on whether it does or not. Do not include anything else in your response."));
                
                if(ability.healing)
                {
                    string result = Testing.GetResponse($"You have started creating a new character with the ability {ability.name} and the description of \"{ability.description}\". Please respond with the number and type of dice used for determining the healing that this ability does, in the format \"(number of dice)d(size of dice)\". Include nothing in your response but this value.");
                    string[] results = result.Split('d');
                    (int, int) resultingDice = (0, 0);
                    foreach(string s in results)
                    {
                        string actual = s.Replace("(", "").Replace(")", "");
                        if(resultingDice.Item1 == 0)
                        {
                            resultingDice.Item1 = int.Parse(actual);
                        }
                        else
                        {
                            resultingDice.Item2 = int.Parse(actual);
                        }
                    }
                    ability.healingDice = new (int, int)[] { resultingDice };
                }
                else if(ability.damage)
                {
                    string result = Testing.GetResponse($"You have started creating a new character with the ability {ability.name} and the description of \"{ability.description}\". Please respond with the number and type of dice used for determining the healing that this ability does, in the format \"(number of dice)d(size of dice)\". Include nothing in your response but this value.");
                    string[] results = result.Split('d');
                    (int, int) resultingDice = (0, 0);
                    foreach (string s in results)
                    {
                        string actual = s.Replace("(", "").Replace(")", "");
                        if (resultingDice.Item1 == 0)
                        {
                            resultingDice.Item1 = int.Parse(actual);
                        }
                        else
                        {
                            resultingDice.Item2 = int.Parse(actual);
                        }
                    }
                    ability.damageDice = new (int, int)[] { resultingDice };
                }
                characterSheet.abilities.Add(ability);
            }
            count = int.Parse(Testing.GetResponse($"You have started creating a DnD character that is a level {level} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name} and the stats of Strength {characterSheet.strength}, Dexterity {characterSheet.dexterity}, Constitution {characterSheet.constitution}, Intelligence {characterSheet.intelligence}, Wisdom {characterSheet.wisdom}, Charisma {characterSheet.charisma}, and the max health of {characterSheet.maxHealth}, with the character details of \"{characterSheet.details}\". Please respond with how many unique items the character should have, including nothing in your response except for the number of abilities."));
            for(int i = 0; i < count; i++)
            {
                Item item = new Item();
                StringBuilder sb = new StringBuilder($"You have started creating a DnD character that is a level {level} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name}. Please come up with a new item in their inventory");
                if (characterSheet.inventory.Count > 0)
                {
                    sb.Append(" that is not ");
                    for (int j = 0; j < characterSheet.inventory.Count - 1; j++)
                    {
                        sb.Append($"{characterSheet.inventory[j].name}, ");
                    }
                    if (characterSheet.inventory.Count == 1)
                    {
                        sb.Append($"{characterSheet.inventory[0].name}.");
                    }
                    else
                    {
                        sb.Append($"or {characterSheet.inventory[characterSheet.inventory.Count - 1].name}.");
                    }
                }
                else
                {
                    sb.Append(".");
                }
                sb.Append("Include nothing in your response but the name of the item.");
                item.name = Testing.GetResponse(sb.ToString());
                item.description = Testing.GetResponse($"Please respond with a description of the item {item.name}. Include nothing but the description of the item in the response. Attempt to keep your response to 30 words or less. This response should describe what the item looks like and things it could be considered similar to.");
                if (bool.Parse(Testing.GetResponse($"Would the item {item.name} with the description \"{item.description}\" be considered a form of ammunition? Include nothing in your response besides \"true\" or \"false\" depending on whether it would be.")))
                {
                    Ammunition ammo = new Ammunition();
                    ammo.name = item.name;
                    ammo.description = item.description;
                    ammo.damageType = Testing.GetResponse($"What type of damage would the weapon {ammo.name} with the description \"{ammo.description}\" deal? respond with nothing but the damage type.");
                    ammo.quantity = Math.Max(int.Parse(Testing.GetResponse($"How many instances of the item {item.name} would a level {characterSheet.characterLevel} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name} have? Respond with nothing but the number represnting the quantity of the item.")), 1);
                    string result = Testing.GetResponse($"You have started creating a new character with the ammunition type {ammo.name} and the description of \"{ammo.description}\". Please respond with the number and type of dice used for determining the healing that this weapon does, in the format \"(number of dice)d(size of dice)\". Include nothing in your response but this value.");
                    string[] results = result.Split('d');
                    (int, int) resultingDice = (0, 0);
                    foreach (string s in results)
                    {
                        string actual = s.Replace("(", "").Replace(")", "");
                        if (resultingDice.Item1 == 0)
                        {
                            resultingDice.Item1 = int.Parse(actual);
                        }
                        else
                        {
                            resultingDice.Item2 = int.Parse(actual);
                        }
                    }
                    ammo.dice = new (int, int)[] {resultingDice};
                    characterSheet.inventory.Add(ammo);
                }
                else if(bool.Parse(Testing.GetResponse($"Would the item {item.name} with the description \"{item.description}\" be considered a weapon? Include nothing in your response besides \"true\" or \"false\" depending on whether it would be.")))
                {
                    Weapon weapon = new Weapon();
                    weapon.name = item.name;
                    weapon.description = item.description;
                    weapon.ammoType = Testing.GetResponse($"Would the weapon {weapon.name} fire some sort of ammunition? If so respond with only the name of the type of ammunition it would fire. If it is some sort of projectile that does not require another item besides the weapon, or it does not fire some form of ammunition, respond with \"none\".").Trim(); ;
                    if (weapon.ammoType.Trim().ToLower() == "none".ToLower())
                    {
                        weapon.ammoType = "";
                    }
                    string result = Testing.GetResponse($"You have started creating a new character with the weapon {weapon.name} and the description of \"{weapon.description}\". Please respond with the number and type of dice used for determining the healing that this weapon does, in the format \"(number of dice)d(size of dice)\". Include nothing in your response but this value.");
                    string[] results = result.Split('d');
                    (int, int) resultingDice = (0, 0);
                    foreach (string s in results)
                    {
                        string actual = s.Replace("(", "").Replace(")", "");
                        if (resultingDice.Item1 == 0)
                        {
                            resultingDice.Item1 = int.Parse(actual);
                        }
                        else
                        {
                            resultingDice.Item2 = int.Parse(actual);
                        }
                    }
                    weapon.damageDice = new (int, int)[] { resultingDice };
                    weapon.quantity = 1;
                    weapon.damageType = Testing.GetResponse($"What type of damage would the weapon {weapon.name} with the description \"{weapon.description}\" deal? respond with nothing but the damage type.");
                    weapon.owner = characterSheet;
                    characterSheet.inventory.Add(weapon);
                }
                else
                {
                    item.quantity = Math.Max(int.Parse(Testing.GetResponse($"How many instances of the item {item.name} would a level {characterSheet.characterLevel} {characterSheet.race} {characterSheet.characterClass} named {characterSheet.name} have? Respond with nothing but the number represnting the quantity of the item.")), 1);
                    characterSheet.inventory.Add(item);
                }
            }
            return characterSheet;
        }

        public static string SetClass()
        {
            Random random = new Random();
            switch (random.Next(12))
            {
                case 0: return "Barbarian";
                case 1: return "Bard";
                case 2: return "Cleric";
                case 3: return "Druid";
                case 4: return "Fighter";
                case 5: return "Monk";
                case 6: return "Paladin";
                case 7: return "Ranger";
                case 8: return "Rogue";
                case 9: return "Sorcerer";
                case 10: return "Warlock";
                case 11: return "Wizard";
                default: return "Fighter";

            }
        }

        public static int SetHealth(int level, string charClass, int con)
        {
            if (charClass.Contains("Barbarian")) return ((level - 1) * 7) + (level * ((int)Math.Round(((double)con - 10) / 2, 0))) + 12;
            else if (charClass.Contains("Bard")) return ((level - 1) * 5) + (level * ((int)Math.Round(((double)con - 10) / 2, 0))) + 8;
            else if (charClass.Contains("Cleric")) return ((level - 1) * 5) + (level * ((int)Math.Round(((double)con - 10) / 2, 0))) + 8;
            else if (charClass.Contains("Druid")) return ((level - 1) * 5) + (level * ((int)Math.Round(((double)con - 10) / 2, 0))) + 8;
            else if (charClass.Contains("Fighter")) return ((level - 1) * 6) + (level * ((int)Math.Round(((double)con - 10) / 2, 0))) + 10;
            else if (charClass.Contains("Monk")) return ((level - 1) * 5) + (level * ((int)Math.Round(((double)con - 10) / 2, 0))) + 8;
            else if (charClass.Contains("Paladin")) return ((level - 1) * 6) + (level * ((int)Math.Round(((double)con - 10) / 2, 0))) + 10;
            else if (charClass.Contains("Ranger")) return ((level - 1) * 6) + (level * ((int)Math.Round(((double)con - 10) / 2, 0))) + 10;
            else if (charClass.Contains("Rogue")) return ((level - 1) * 5) + (level * ((int)Math.Round(((double)con - 10) / 2, 0))) + 8;
            else if (charClass.Contains("Sorcerer")) return ((level - 1) * 4) + (level * ((int)Math.Round(((double)con - 10) / 2, 0))) + 6;
            else if (charClass.Contains("Warlock")) return ((level - 1) * 5) + (level * ((int)Math.Round(((double)con - 10) / 2, 0))) + 8;
            else if (charClass.Contains("Wizard")) return ((level - 1) * 4) + (level * ((int)Math.Round(((double)con - 10) / 2, 0))) + 6;
            else return ((level - 1) * 5) + (level * ((int)Math.Round(((double)con - 10) / 2, 0))) + 8;
        }
    }
}
