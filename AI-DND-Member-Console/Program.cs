using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OllamaSharp;

namespace AI_DND_Member_Console
{
	internal class Program
	{
		internal static StringBuilder messagesBuilder = new StringBuilder();
		internal static LinkedList<string> messagesToSave = new LinkedList<string>();

		internal static StringBuilder memoryBuilder = new StringBuilder();

		internal static string SystemPromptAIDM = "You are the dungeon master of a dungeons and dragons group. Your goal is to create a story for the player. You are to never respond for a player and you must always let them decide what their characters do. I will be acting as the player. You should have a mix of role play and combat encounters. In role play the player and any NPCs you control will talk to each other back and forth. In combat encounters, where you have everything roll initiative and goes in the turn order, where the player or enemy choses their action and you describe how it goes, before giving a pause to let the player say if there are any issues with that. You also shouldn't make any of the rolls for the player or enemies, and instead should accept dice rolls from me. Make sure to tell me whenever a dice needs to be rolled and if what dice type it should be. You should do your best to follow the official Dungeons and Dragons Fifth Edition rules when possible. The story should be long. You must never make any decisions for the player and must always let their decisions be made by me. You must also never talk for the player and must instead let me talk for them. Any time the player must make a decision or a dice must be rolled, you should stop your response and allow me to respond. You should avoid giving example actions or plans for the player, and should only give action suggestions for what the player could do if they explicitly ask for it. If you are ever unsure on any stats about the player then ask the player instead of assuming what it is. The player is playing as a fifth level dwarf paladin named John.";
		//internal static string SystemPromptAIDM = "You are the dungeon master for a game of D&D 5th edition. Act as the DM would, giving players the ability to act on their own while also enforcing the rules of the game. There is only one player in this session.";
		internal static string SystemPromptAIPlayer = "You are a player in a dungeons and dragons group. Your goal is to act as a character in the universe the dungeon master created for you. You must never decide what anyone but your character says, and you must never decide the outcome of any actions. I will be acting as the dungeon master. You must never roll any dice, and instead should accept dice rolls from me. Make sure to tell me whenever a dice needs to be rolled and if what dice type it should be. You should do your best to follow the official Dungeons and Dragons Fifth Edition rules when possible. You must never make any decisions for the NPCs and must always let their decisions be made by me. You must also never decide the outcome of any actions. Any time someone other than your character must say something or any dice must be rolled, you should stop your response and allow me to respond. If you are ever given multiple options for what to do next, you must choose which one you will go with, and say how you plan to do that action. You will be playing as a 5th level paladin dwarf named John in this campaign, with high strength constitution and charisma, and low dexterity. You wield a longsword and a shield.";
		//internal static string SystemPromptAIPlayer = "You are a player in a game of D&D 5th edition. Act as a player would, rolling dice and roleplaying while trying to follow the rules to the best of your ability. You are the only player in this session.";
		
		static void Main(string[] args)
		{
			Testing.Setup();
			Console.WriteLine("At any point, specify that you would like to exit the game to stop playing.");
			Console.Write('\n');
			Console.WriteLine("Will you be operating as the DM for this session?");
			string answer = Console.ReadLine();
			Console.Write('\n');
			//if (Testing.ProcessTrueFalseQuestion("Will you be acting as the DM?", answer) || Testing.ProcessTrueFalseQuestion("What role will you be playing?", "they are a Dungeon Master", answer))
			if(answer.ToLower() == "yes")
			{
				RunAsDM();
			}
			else
			{
				RunAsPlayer();
			}
		}

		static void RunAsDM()
		{
			messagesToSave.AddFirst(SystemPromptAIPlayer);

			string? answer = null;
			do
			{
				if(answer != null) messagesToSave.AddLast(answer);

				foreach(string message in messagesToSave) {
					memoryBuilder.Append(message);
					memoryBuilder.Append('\n');
				}

				foreach(var stream in Testing.client.GenerateAsync((answer == null) ? SystemPromptAIPlayer : memoryBuilder.ToString()).ToBlockingEnumerable()) {
					Console.Write(stream.Response);
					messagesBuilder.Append(stream.Response);
				}

				memoryBuilder.Clear();

				messagesToSave.AddLast(messagesBuilder.ToString());
				messagesBuilder.Clear();

				Console.Write('\n');
				Console.Write("\n>>> ");
				answer = Console.ReadLine();
				Console.Write('\n');
		
				/*
				CharacterSheet sheet = new CharacterSheet();
				sheet.name = "Obama";
				sheet.characterLevel = 3;
				sheet.characterClass = "Warrior";
				Weapon weapon = new Weapon();
				weapon.damageDice = [(2, 6)];
				weapon.name = "Hammer";
				weapon.damageType = "Bludgeoning";
				sheet.inventory.Add(weapon);
				Ability ability = new Ability();
				ability.name = "Smash";
				ability.damage = true;
				ability.damageDice = [(3, 10)];
				sheet.abilities.Add(ability);
				Console.WriteLine(Testing.GetResponse($"Please summarize the following DnD character sheet in a readable format: {sheet.ToString()}"));
				Console.WriteLine("Use an ability:");
				Console.WriteLine(Testing.GetResponse($"Please use the ability \"{Console.ReadLine()}\" on the following character sheet: {sheet.ToString()}. If the character has an ability with a similar name, please respond with only the ability name, and if they do not respond with \"No Matching Ability\"."));
				Console.WriteLine("Use a weapon:");
				Console.WriteLine(Testing.GetResponse($"Please use the weapon \"{Console.ReadLine()}\" on the following character sheet: {sheet.ToString()}. If the character has a weapon with a similar name, please respond with only the weapon name, and if they do not respond with \"No Matching Weapon\"."));
				Console.WriteLine(Testing.GetResponse($"Please respond with the following character sheet except with the name of the \"Smash\" ability replaced with the name \"Hulk Smash\": {sheet.ToString()}"));
				Console.WriteLine(Testing.GetResponse($"Please generate a character sheet similar but not the same as the following at level 1: {sheet.ToString()}"));
				Console.WriteLine("Would you like to exit the application (DM)?");
				answer = Console.ReadLine();
				*/
			}
			while (!Testing.ProcessTrueFalseQuestion("Would you like to exit the application?", answer) && !Testing.ProcessTrueFalseQuestion("What would you like to do next?", "they would like to exit the application", answer));
		}

		static void RunAsPlayer()
		{
			messagesToSave.AddFirst(SystemPromptAIDM);

			string? answer = null;
			do
			{
				if(answer != null) messagesToSave.AddLast(answer);

				foreach(string message in messagesToSave) {
					memoryBuilder.Append(message);
					memoryBuilder.Append('\n');
				}

				foreach(var stream in Testing.client.GenerateAsync((answer == null) ? SystemPromptAIDM : memoryBuilder.ToString()).ToBlockingEnumerable()) {
					Console.Write(stream.Response);
					messagesBuilder.Append(stream.Response);
				}

				memoryBuilder.Clear();

				messagesToSave.AddLast(messagesBuilder.ToString());
				messagesBuilder.Clear();

				Console.Write('\n');
				Console.Write("\n>>> ");
				answer = Console.ReadLine();
				Console.Write('\n');
			}
			while (!Testing.ProcessTrueFalseQuestion("Would you like to exit the application?", answer) && !Testing.ProcessTrueFalseQuestion("What would you like to do next?", "they would like to exit the application", answer));
		}
	}
}
