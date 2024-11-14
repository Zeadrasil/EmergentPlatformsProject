using System;
using System.Threading.Tasks;
using OllamaSharp;

namespace AI_DND_Member_Console {
	internal class Program {
		static void Main(string[] args) {
			Console.WriteLine("This is a chat space where you can interact with an AI and use it to play D&D,");
			Console.WriteLine("with the AI serving either as a player or as a DM.");
			Console.WriteLine("At any point, type \"/bye\" to end the chat.");
			Console.Write('\n');

			// Setup the client
			Uri uri = new Uri("http://localhost:11434");
			OllamaApiClient client = new OllamaApiClient(uri);
			client.SelectedModel = "qwen2.5:7b";

			// Setup the chat
			Chat chat = new Chat(client);

			while(true) {
				Console.Write(">> ");
				string message = Console.ReadLine();

				if(message == "/bye") {
					break;
				}

				Console.Write('\n');
				foreach(var answerToken in chat.SendAsync(message).ToBlockingEnumerable()) {
					Console.Write(answerToken);
				}
				Console.Write("\n\n");
			}
		}
	}
}
