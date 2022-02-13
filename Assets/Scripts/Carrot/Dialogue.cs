using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	[System.Serializable]
	public class Dialogue {

		private string name;

		[TextArea(3, 10)]
		public Queue<(string, string)> sentences;

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public Queue<(string, string)> Sentences {
			get { return sentences; }
			set { sentences = value; }
		}

	}
}