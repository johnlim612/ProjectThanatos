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
		public Animator animator;

		private Queue<(string, string)> sentences;
		private bool promptSelected = false;
		private Dialogue dialogue;


		// Start is called before the first frame update
		void Start() {
			sentences = new Queue<(string, string)>();
		}

		public void SelectDialogue() {
			
			promptSelected = true;

		}

		
		public void CreatePrompts() {
			//string[] prompts = DialogueManager.GetPrompts(itemName);
			// CreateButtons
			// ShowPrompts
		}

		public void StartDialogue(string itemName) {

			// Prompt Greeting here:
			// __.getGreeting(itemName)

			//check if it is person or item. If it is a person:
			//make methods for each type person, item, and diary

			// For person
			// DialogueManager.GetDialogue(int stringNumber)

			CreatePrompts();
			CreateDialog();
			nameText.text = dialogue.name;

			sentences.Clear();
			 
			foreach ((string, string) sentence in dialogue.sentences) {
				sentences.Enqueue(sentence);
				//if key == newPrompt then use recursion to start method again
			}

			DisplayNextSentence();
		}

		public void CreateDialogue(string itemName, Queue<(string, string)> Dialogues) {
			dialogue = new Dialogue();
			dialogue.name

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