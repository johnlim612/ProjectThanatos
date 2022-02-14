using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class UIDialogueManager: MonoBehaviour {
		[Header("DialogueBoxes")]
		public GameObject DefaultDialogue;
		public Button[] buttons;

		public Text NameText;
		public Text DialogueText;
		public Animator Animator;
		private Dialogue _dialogue;

		// Temporary holder for current dialogue queue
		private Queue<(string, string)> _sentences;
		private bool _promptSelected = false;
		private int _promptSelection = 0;
		GameObject player = GameObject.Find("Player");

		// Start is called before the first frame update
		void Start() {
			_sentences = new Queue<(string, string)>();
		}

		public void SelectPrompt(int buttonIndex) {
			foreach (Button button in buttons) {
				button.gameObject.SetActive(false);
			}
			_promptSelection = buttonIndex;
			_promptSelected = true;
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
			_sentences.Clear();
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
			while (_promptSelected == false) {
				yield return null;
			}
			yield return new WaitForSeconds(0.5f);
			_promptSelected = false;
		}

		public void CreateDialogue(GameObject item) {
			_dialogue = new Dialogue();
			_dialogue.Name = item.name;
			//dialogue.Sentences = DialogueManager.GetDialogue(promptSelection);
		}

		public void SimulateDialogue() {
			foreach ((string, string) sentence in _dialogue.Sentences) {
				_sentences.Enqueue(sentence);
				//if _sentences.item1 == "New_Prompt" then run start dialogue again to
				//start method again for new prompts during conversation
			}
			DisplayNextSentence();
		}

		public void DisplayNextSentence() {
			// If dialogue has endedM
			if (_sentences.Count == 0) {
				EndDialogue();
				return;
			}

			(string, string) sentence = _sentences.Dequeue();
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
