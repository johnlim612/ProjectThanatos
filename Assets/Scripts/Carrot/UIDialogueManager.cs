using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class UIDialogueManager: MonoBehaviour {
		[Header("DialogueBoxes")]
		public GameObject defaultDialogue;
		public GameObject narrativeDialogue;
		public GameObject intenseDialogue;

		public Text nameText;
		public Text dialogueText;
		private Queue<(string, string)> sentences;
		public Animator animator;		


		// Start is called before the first frame update
		void Start() {
			sentences = new Queue<(string, string)>();
		}

		public void ContinueDialog() {
				
		}

		public void ShowPrompts() {
			//show user prompts
		}

		public void StartDialogue(string itemName) {

			// Prompt Greeting here:
			// __.getGreeting(itemName)

			//check if it is person or item. If it is a person:
			// __.GetPrompts
			ShowPrompts();
			CreateDialog();
			nameText.text = dialogue.name;

			sentences.Clear();
			 
			foreach (string sentence in dialogue.sentences) {
				sentences.Enqueue(sentence);
			}

			DisplayNextSentence();
		}

		public void DisplayNextSentence() {
			if (sentences.Count == 0) {
				EndDialogue();
				return;
			}
			string sentence = sentences.Dequeue();
			StopAllCoroutines();
			StartCoroutine(TypeSenetence(sentence));
		}

		IEnumerator TypeSenetence (string sentence) {
			dialogueText.text = "";
			foreach (char letter in sentence.ToCharArray()) {
				dialogueText.text += letter;
				yield return new WaitForSeconds(0.05f);
			}
		}

		void EndDialogue() {
			animator.SetBool("isOpen", false);
		}

	}
}