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

		internal static string SystemPromptAIDM = "You are the dungeon master for a game of D&D 5th edition. Act as the DM would, giving players the ability to act on their own while also enforcing the rules of the game. There is only one player in this session.";
		internal static string SystemPromptAIPlayer = "You are a player in a game of D&D 5th edition. Act as a player would, rolling dice and roleplaying while trying to follow the rules to the best of your ability. You are the only player in this session.";
		
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

				foreach(var stream in Testing.client.GenerateAsync((answer == null) ? SystemPromptAIPlayer : answer).ToBlockingEnumerable()) {
					Console.Write(stream.Response);
					messagesBuilder.Append(stream.Response);
				}

				messagesToSave.AddLast(messagesBuilder.ToString());
				messagesBuilder.Clear();

				Console.Write('\n');
				Console.Write("\n>>> ");
				answer = Console.ReadLine();
				Console.Write('\n');
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

				foreach(var stream in Testing.client.GenerateAsync((answer == null) ? SystemPromptAIDM : answer).ToBlockingEnumerable()) {
					Console.Write(stream.Response);
					messagesBuilder.Append(stream.Response);
				}

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
