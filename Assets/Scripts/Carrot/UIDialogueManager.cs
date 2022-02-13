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

		private Dialogue dialogue;

		// Temporary holder for current dialogue queue
		private Queue<(string, string)> sentences;
		private bool promptSelected = false;
		private int promptSelection = 0;


		// Start is called before the first frame update
		void Start() {
			sentences = new Queue<(string, string)>();
		}

		public void SelectPrompt() {
			
			promptSelected = true;
			//button returns index replace 1 with it
			promptSelection = 1;
		}

		public void StartDialogue(object item) {
			// Pause movement here:

			// Prompt Greeting here:
			// __.getGreeting(itemName)

			// Check if it is person or item. If it is a person:
			// make methods for each type character, item, and diary

			// For Character type

			//remove paranthesis for item.name
			CreatePrompts("item.name");
			WaitForUserPrompt();
			// Create Dialogue Object
			CreateDialogue(item);
			sentences.Clear();
			SimulateDialogue();
		}

		public void CreatePrompts(string name) {
			//string[] prompts = DialogueManager.GetPrompts(name);
			// CreateButtons
			// ShowPrompts
		}

		IEnumerator WaitForUserPrompt() {

			while (promptSelected == false) {
				yield return null;
			}
		}

		public void CreateDialogue(object item) {
			dialogue = new Dialogue();
			dialogue.Name = item.name;
			dialogue.Sentences = DialogueManager.GetDialogue(promptSelection);
		}

		public void SimulateDialogue() {
			foreach ((string, string) sentence in dialogue.sentences) {
				sentences.Enqueue(sentence);
				//if sentences.item1 == "New_Prompt" then use recursion to
				//start method again for new prompts during conversation
			}

			DisplayNextSentence();
		}

		public void DisplayNextSentence() {
			
			// If dialogue has ended
			if (sentences.Count == 0) {
				EndDialogue();
				return;
			}

			(string, string) sentence = sentences.Dequeue();
			StopAllCoroutines();
			StartCoroutine(TypeSenetence(sentence));
		}

		IEnumerator TypeSenetence ((string, string) sentence) {
			
			nameText.text = sentence.Item1;
			
			// Reset dialogue text
			dialogueText.text = "";

			foreach (char letter in sentence.Item2.ToCharArray()) {
				dialogueText.text += letter;
				yield return new WaitForSeconds(0.05f);
			}
		}

		void EndDialogue() {
			animator.SetBool("isOpen", false);
			// renable movement here:
		}

	}
}