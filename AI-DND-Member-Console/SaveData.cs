using System.Collections.Generic;

namespace AI_DND_Member_Console {
	internal class SaveData {
		public bool isDM {get; set;}
		public List<string> summarizedData {get; set;}
		public string rawData {get; set;}

		public SaveData() {
			isDM = false;
			summarizedData = new List<string>();
			rawData = string.Empty;
		}
	}
}
