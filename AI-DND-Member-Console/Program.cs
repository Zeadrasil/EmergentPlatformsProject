using System;
using System.Threading.Tasks;
using OllamaSharp;

namespace AI_DND_Member_Console {
	internal class Program {
		static void Main(string[] args) {
			// Setup the client
			Uri uri = new Uri("http://localhost:11434");
			OllamaApiClient client = new OllamaApiClient(uri);
			client.SelectedModel = "qwen2.5:7b";

			// Setup the chat
			Chat chat = new Chat(client);

			while(true) {
				string message = Console.ReadLine();

				if(message == "/bye") {
					break;
				}

				foreach(var answerToken in chat.SendAsync(message).ToBlockingEnumerable()) {
					Console.Write('\n');
					Console.Write(answerToken);
				}
				Console.Write("\n\n");

				Console.WriteLine("Type \"/bye\" to end");
			}
		}
	}
}
