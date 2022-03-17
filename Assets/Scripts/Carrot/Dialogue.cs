using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	[System.Serializable]
	public class Dialogue {

		private string _name;

		[TextArea(3, 10)]
		private Queue<(string, string)> _sentences;

		public string Name {
			get { return _name; }
			set { _name = value; }
		}

		public Queue<(string, string)> Sentences {
			get { return _sentences; }
			set { _sentences = value; }
		}
	}
}