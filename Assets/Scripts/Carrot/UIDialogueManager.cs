using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class UIDialogueManager: MonoBehaviour {
		public static bool IsInteracting = false;

		[Header("DialogueBoxes")]
		public GameObject DefaultDialogue;
		public Button[] Buttons;

		public Text NameText;
		public Text DialogueText;
		public Button NextButton;
		public Animator Animator;
		public GameObject Dialogue;
		private Dialogue _dialogue;

		// Temporary holder for current dialogue queue
		private Queue<(string, string)> _sentences;
		private bool _promptSelected = false;
		private int _promptSelection = 0;
		private int _maxPrompts = 4;
		private GameObject _player;

		// Start is called before the first frame update
		void Awake() {
			_sentences = new Queue<(string, string)>();
			_player = GameObject.Find(Constants.PlayerKey);
			ToggleNextButton(true);

		}

		public void SelectPrompt(int buttonIndex) {
			foreach (Button button in Buttons) {
				button.gameObject.SetActive(false);
			}
			_promptSelection = buttonIndex;
			_promptSelected = true;
		}

		public void ToggleNextButton(bool toggle) {
			NextButton.enabled = toggle;
			if (toggle) {
				NextButton.GetComponentInChildren<Text>().text = "Continue";
			} 
		}

		public void StartSystemAlert(Queue<(string, string)> sysAnnounce) {
			_player.GetComponent<PlayerController>().enabled = false;
			IsInteracting = true;
			_sentences = sysAnnounce;
			//print(_sentences.Count + " at StartDiaryDialogue()");
			Animator.SetBool("IsOpen", true);
			DisplayNextSentence();
		}

		public void StartDiaryDialogue(Diary diary) {
			_player.GetComponent<PlayerController>().enabled = false;
			IsInteracting = true;
			_sentences = diary.DescriptionQueue;
			Animator.SetBool("IsOpen", true);
			DisplayNextSentence();
		}

		public void StartAnnouncement() {
			DialogueDataManager.Initialize(DataType.SystemAnnouncement, 
				Constants.SystemAnnouncement, GameManager.SabotageId);
			_sentences = DialogueDataManager.GetAnnouncement();
			Animator.SetBool("IsOpen", true);
            DisplayNextSentence();
        }


		public void StartItemDialogue(Item item) {
			IsInteracting = true;
			_sentences = item.DescriptionQueue;
			Animator.SetBool("IsOpen", true);
			DisplayNextSentence();
		}

		public void StartDialogue(NPC npc) {

			// Pause movement here:
			_player.GetComponent<PlayerController>().enabled = false;
			IsInteracting = true;

			// Pause next button
			NextButton.enabled = false;
			
			// Find and Load all Data pertaining to the characters' dialogue.
			if (npc.HasBeenSpokenTo) {
				DialogueDataManager.Initialize(DataType.CharacterDialogue, npc.gameObject.name);
			} else {
				DialogueDataManager.Initialize(DataType.CharacterDialogue, npc.gameObject.name, 
				npc.CountCharDialogue);
				npc.UpdateCharDialogueProgress();
			}


			// Prompt Greeting here:
			StartCoroutine(TypeSentence((npc.gameObject.name, DialogueDataManager.GetGreeting())));
			
			Animator.SetBool("IsOpen", true);
			InitializePrompts();
			DisplayPrompts(npc.name);
			StartCoroutine(WaitForUserPrompt(npc));
            // Create Dialogue Object
        }

		public void ContinueDialogue(NPC item) {
			ToggleNextButton(true);
			CreateDialogue(item);
			SimulateDialogue();
		}

		public void InitializePrompts() {
			for (int i = 0; i < Buttons.Length; i ++) {
				Button button = Buttons[i];
				int buttonIndex = i;
				button.onClick.AddListener(() => SelectPrompt(buttonIndex));
			}
		}

		public void DisplayPrompts(string name) {
			List<string> prompts = DialogueDataManager.GetPrompts();
			for (int i = 0;  i < Buttons.Length; i++) {
				if (i >= prompts.Count) {
					Buttons[i].gameObject.SetActive(false);
					continue;
				}
				Buttons[i].gameObject.SetActive(true);
				Buttons[i].GetComponentInChildren<Text>().text = prompts[i];
			}
		}

		IEnumerator WaitForUserPrompt(NPC item) {
			while (!_promptSelected) {
				yield return null;
			}
			yield return new WaitForSeconds(0.1f);
			_promptSelected = false;
			ContinueDialogue(item);
		}

		public void CreateDialogue(InteractableObject item) {
			_dialogue = new Dialogue();
			_dialogue.Name = item.name;
			_dialogue.Sentences = DialogueDataManager.GetDialogue(_promptSelection);
		}

		public void SimulateDialogue() {
			_sentences.Clear();
			foreach ((string, string) sentence in _dialogue.Sentences) {
				_sentences.Enqueue(sentence);
				//if _sentences.item1 == "New_Prompt" then run start dialogue again to
				//start method again for new prompts during conversation
			}
			DisplayNextSentence();
		}

		public void DisplayNextSentence() {
			//print(_sentences.Count + " at DisplayNextSentence()");
			// If dialogue has endedM
			if (_sentences.Count == 0) {
				EndDialogue();
				return;
			}

			(string, string) sentence = _sentences.Dequeue();
			StopAllCoroutines();
			StartCoroutine(TypeSentence(sentence));
		}

		IEnumerator TypeSentence((string, string) sentence) {
			NameText.text = sentence.Item1;
			DialogueText.text = "";

			foreach (char letter in sentence.Item2.ToCharArray()) {
				DialogueText.text += letter;
				yield return new WaitForSeconds(0.02f);
			}
		}

		void EndDialogue() {
			//print(Animator.GetBool("IsOpen"));
			Animator.SetBool("IsOpen", false);
			_player.GetComponent<PlayerController>().enabled = true;
			IsInteracting = false;
		}
	}
}
