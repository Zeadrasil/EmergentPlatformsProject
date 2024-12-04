using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OllamaSharp;

namespace AI_DND_Member_Console
{
	internal class Program
	{
		private static StringBuilder messagesBuilder = new StringBuilder();
		internal static LinkedList<string> messagesToSave = new LinkedList<string>();

		private static StringBuilder memoryBuilder = new StringBuilder();

		private static string SystemPromptAIDM = "You are the dungeon master for a game of D&D 5th edition. Act as the DM would, giving players the ability to act on their own while also enforcing the rules of the game. There is only one player in this session.";
		private static string SystemPromptAIPlayer = "You are a player in a game of D&D 5th edition. Act as a player would, rolling dice and roleplaying while trying to follow the rules to the best of your ability. You are the only player in this session.";

		private const string SAVE_FILE_LOCATION = "../../../saves";
		private static SaveData? saveFile = null;
		private static string saveFileName = string.Empty;

		static void Main(string[] args)
		{
			Testing.Setup();

			if(!Directory.Exists(SAVE_FILE_LOCATION))
			{
				Directory.CreateDirectory(SAVE_FILE_LOCATION);
			}

			Console.WriteLine("At any point, specify that you would like to exit the game to stop playing.");
			Console.Write('\n');
			if(Directory.GetFiles(SAVE_FILE_LOCATION).Length > 0) {
				Console.WriteLine("Is there a save file you would like to load?");
				string loadAnswer = Console.ReadLine();
				Console.Write('\n');

				if(loadAnswer.ToLower() == "yes" || loadAnswer.ToLower() == "y") {
					Console.WriteLine("What is the name of this save file?");
					string? input = null;
					while(input == null) {
						input = Console.ReadLine();

						// Ensure the file name is valid
						if(input != null && input.IndexOfAny(Path.GetInvalidPathChars()) >= 0) {
							input = null;
						}
					}

					string json = File.ReadAllText($"{SAVE_FILE_LOCATION}/{input}.json");
					saveFileName = input;

					saveFile = JsonSerializer.Deserialize<SaveData>(json);
				}
			}

			if(saveFile == null)
			{
				Console.WriteLine("Will you be operating as the DM for this session?");
				string answer = Console.ReadLine();
				Console.Write('\n');
				if(answer.ToLower() == "yes" || answer.ToLower() == "y")
				{
					RunAsDM();
				}
				else
				{
					RunAsPlayer();
				}
			}
			else
			{
				Console.Write('\n');
				Console.WriteLine(saveFile.rawData);

				if(saveFile.isDM)
				{
					memoryBuilder.AppendLine(SystemPromptAIPlayer);

					foreach(string data in saveFile.summarizedData) {
						memoryBuilder.AppendLine(data);
					}

					RunAsDM();
				}
				else
				{
					memoryBuilder.AppendLine(SystemPromptAIDM);

					foreach(string data in saveFile.summarizedData) {
						memoryBuilder.AppendLine(data);
					}

					RunAsPlayer();
				}
			}
		}

		static void RunAsDM()
		{
			//messagesToSave.AddFirst(SystemPromptAIPlayer);

			string? answer = null;
			do
			{
				if(answer != null)
				{
					//messagesToSave.AddLast(answer);
					memoryBuilder.AppendLine(answer);
				}

				foreach(var stream in Testing.client.GenerateAsync((answer == null) ? SystemPromptAIPlayer : memoryBuilder.ToString()).ToBlockingEnumerable()) {
					Console.Write(stream.Response);
					messagesBuilder.Append(stream.Response);
				}

				//messagesToSave.AddLast(messagesBuilder.ToString());
				memoryBuilder.AppendLine(messagesBuilder.ToString());
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
			//messagesToSave.AddFirst(SystemPromptAIDM);

			bool skipFirstPrompt = saveFile != null;

			string? answer = null;
			do
			{
				if(answer != null)
				{
					messagesToSave.AddLast(answer);
					memoryBuilder.AppendLine(answer);
				}

				if(!skipFirstPrompt)
				{
					foreach(var stream in Testing.client.GenerateAsync((answer == null) ? SystemPromptAIDM : memoryBuilder.ToString()).ToBlockingEnumerable()) {
						Console.Write(stream.Response);
						messagesBuilder.Append(stream.Response);
					}

					messagesToSave.AddLast(messagesBuilder.ToString());
					memoryBuilder.AppendLine(messagesBuilder.ToString());
					messagesBuilder.Clear();
				}
				skipFirstPrompt = false;

				Console.Write('\n');
				Console.Write("\n>>> ");
				answer = Console.ReadLine();
				Console.Write('\n');
			}
			while (!Testing.ProcessTrueFalseQuestion("Would you like to exit the application?", answer) && !Testing.ProcessTrueFalseQuestion("What would you like to do next?", "they would like to exit the application", answer));

			StringBuilder textToSummarize = new StringBuilder();
			foreach(string message in messagesToSave)
			{
				textToSummarize.AppendLine(message);
			}

			StringBuilder summaryBuilder = new StringBuilder();
			foreach(var stream in Testing.client.GenerateAsync($"Summarize the following string: {textToSummarize}").ToBlockingEnumerable())
			{
				summaryBuilder.Append(stream.Response);
			}

			if(saveFile == null)
			{
				saveFile = new SaveData();
				saveFile.summarizedData.Add(summaryBuilder.ToString());
				saveFile.rawData = memoryBuilder.ToString();

				JsonSerializerOptions options = new JsonSerializerOptions();
				options.WriteIndented = true;

				string json = JsonSerializer.Serialize(saveFile, options);

				Console.WriteLine("What would you like to name this save file?");
				string? input = null;
				while(input == null)
				{
					input = Console.ReadLine();

					// Ensure the file name is valid
					if(input != null && input.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
					{
						input = null;
					}
				}

				File.WriteAllText($"{SAVE_FILE_LOCATION}/{input}.json", json);
			}
			else
			{
				saveFile.summarizedData.Add(summaryBuilder.ToString());
				saveFile.rawData = memoryBuilder.ToString();

				JsonSerializerOptions options = new JsonSerializerOptions();
				options.WriteIndented = true;

				string json = JsonSerializer.Serialize(saveFile, options);

				File.WriteAllText($"{SAVE_FILE_LOCATION}/{saveFileName}.json", json);
			}
		}
	}
}
