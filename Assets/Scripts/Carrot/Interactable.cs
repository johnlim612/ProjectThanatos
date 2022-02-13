using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	public class Interactable: MonoBehaviour {
		public string itemName;

		public void TriggerDialogue() {
			FindObjectOfType<UIDialogueManager>().StartDialog(itemName);
		}
	}
}