using System;
using System.Threading.Tasks;
using OllamaSharp;

namespace AI_DND_Member_Console {
	internal class Testing {
		public void Run() {
			// Setup the client
			Uri uri = new Uri("http://localhost:11434");
			OllamaApiClient client = new OllamaApiClient(uri);

			//var models = client.ListLocalModelsAsync();

			//foreach(Model model in models.Result) {
			//	Console.WriteLine(model.Name);
			//}

			client.SelectedModel = "qwen2.5:7b";

			foreach(var stream in client.GenerateAsync("What are you?").ToBlockingEnumerable()) {
				Console.Write(stream.Response);
			}
		}
	}
}
