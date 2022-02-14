using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class UIDialogueManager: MonoBehaviour {
		[Header("DialogueBoxes")]
		public GameObject defaultDialogue;
		public Button[] buttons;

		public Text nameText;
		public Text dialogueText;
		public Animator animator;
		private Dialogue dialogue;

		// Temporary holder for current dialogue queue
		private Queue<(string, string)> sentences;
		private bool promptSelected = false;
		private int promptSelection = 0;
		GameObject player = GameObject.Find("Player");

		// Start is called before the first frame update
		void Start() {
			sentences = new Queue<(string, string)>();
		}

		public void SelectPrompt(int buttonIndex) {
			foreach (Button button in buttons) {
				button.gameObject.SetActive(false);
			}
			promptSelection = buttonIndex;
			promptSelected = true;
		}

		public void StartDialogue(GameObject item) {
			// Pause movement here:
			player.GetComponent<PlayerController>().enabled = false;
			// Prompt Greeting here:
			// __.getGreeting(itemName)

			// Check if it is person or item. If it is a person:
			// make methods for each type character, item, and diary

			// For Character type

			//remove paranthesis for item.name
			InitializePrompts();
			DisplayPrompts(item.name);
			WaitForUserPrompt();
			// Create Dialogue Object
			CreateDialogue(item);
			sentences.Clear();
			SimulateDialogue();
		}
		public void InitializePrompts() {
			for (int i = 0; i < buttons.Length; i ++) {
				Button button = buttons[i];
				int buttonIndex = i;
				button.onClick.AddListener(() => SelectPrompt(buttonIndex));
			}
		}

		public void DisplayPrompts(string name) {
			//string[] prompts = DialogueManager.GetPrompts(name);
			// CreateButtons
			for (int i = 0;  i < buttons.Length; i++) {
				if (i >= prompts.Length) {
					buttons[i].gameObject.SetActive(false);
					continue;
				}
				buttons[i].gameObject.SetActive(true);
				buttons[i].GetComponentInChildren<Text>().text = prompts[i];
			}
		}

		IEnumerator WaitForUserPrompt() {
			while (promptSelected == false) {
				yield return null;
			}
			yield return new WaitForSeconds(0.5f);
			promptSelected = false;
		}

		public void CreateDialogue(GameObject item) {
			dialogue = new Dialogue();
			dialogue.Name = item.name;
			//dialogue.Sentences = DialogueManager.GetDialogue(promptSelection);
		}

		public void SimulateDialogue() {
			foreach ((string, string) sentence in dialogue.sentences) {
				sentences.Enqueue(sentence);
				//if sentences.item1 == "New_Prompt" then run start dialogue again to
				//start method again for new prompts during conversation
			}
			DisplayNextSentence();
		}

		public void DisplayNextSentence() {
			// If dialogue has endedM
			if (sentences.Count == 0) {
				EndDialogue();
				return;
			}

			(string, string) sentence = sentences.Dequeue();
			StopAllCoroutines();
			StartCoroutine(TypeSenetence(sentence));
		}

		IEnumerator TypeSenetence((string, string) sentence) {
			nameText.text = sentence.Item1;
			dialogueText.text = "";

			foreach (char letter in sentence.Item2.ToCharArray()) {
				dialogueText.text += letter;
				yield return new WaitForSeconds(0.05f);
			}
		}

		void EndDialogue() {
			animator.SetBool("isOpen", false);
			player.GetComponent<PlayerController>().enabled = true;
		}
	}
}