﻿using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OllamaSharp;

namespace AI_DND_Member_Console 
{
	public static class Testing 
	{
		//Store client so that it can be used universally
		private static OllamaApiClient client;

		public static void Run() 
		{
			// Setup the client
			Uri uri = new Uri("http://localhost:11434");
			client = new OllamaApiClient(uri);

			//var models = client.ListLocalModelsAsync();

			//foreach(Model model in models.Result) {
			//	Console.WriteLine(model.Name);
			//}

			client.SelectedModel = "qwen2.5:7b";

			foreach(var stream in client.GenerateAsync("What are you?").ToBlockingEnumerable()) {
				Console.Write(stream.Response);
			}
			Console.WriteLine();
		}

        public static bool ProcessTrueFalseQuestion(string question, string answer)
        {
            //Generate the actual prompt to use to ask the llm
            string prompt = $"Please respond to the following statement with either the text \"false\" or \"true\". The following statement in response to the question \"{question}\" is more likely to indicate the affirmative than not: {answer}";

            //Get the response from the llm
            foreach (var stream in client.GenerateAsync(prompt).ToBlockingEnumerable())
            {
                if (stream.Response == "true")
                {
                    return true;
                }
            }
            return false;
        }
        public static bool ProcessTrueFalseQuestion(string question, string correctEquivalent, string answer)
        {
            //Generate the actual prompt to use to ask the llm
            string prompt = $"Please respond to the following statement with either the text \"false\" or \"true\". The following statement in response to the question \"{question}\" is more likely to indicate that {correctEquivalent} than not: {answer}";

            //Get the response from the llm
            foreach (var stream in client.GenerateAsync(prompt).ToBlockingEnumerable())
            {
                if (stream.Response == "true")
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetResponse(string question)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var stream in client.GenerateAsync(question).ToBlockingEnumerable())
            {
                sb.Append(stream.Response);
            }
            return sb.ToString();
        }
    }
}
