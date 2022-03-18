using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class UIDialogueManager: MonoBehaviour {

		public GameObject DialogueUIObject;
		
		private DialogueUI _dialogueUI;
		private Dialogue _dialogue;

		// Temp Dialogue queue holder
		private Queue<(string, string)> _sentences;

		// Waits for user to select prompt
		private bool _promptSelected = false;
		private int _promptSelection = 0;
		private GameObject _player;

		private const int _maxPrompts = 4;
		private const bool _v = false;
		private const float _sentenceSpeed = 0.02f;

		public static bool IsInteracting { get; set; } = _v;

		// Start is called before the first frame update
		void Awake() {
			_dialogueUI = DialogueUIObject.GetComponent<DialogueUI>();
			_sentences = new Queue<(string, string)>();
			_player = GameObject.Find(Constants.PlayerKey);
		}

		public void InitializeDialogue(Interactables interactable, InteractableObject entity, Queue<(string, string)> sysAnnounce = null) {
			// If entity is not registered
			if (!Enum.IsDefined(typeof(Interactables), interactable)) {
				return;
				// item not recognized
			}

			PrepareDialogue(interactable);

			switch (interactable) {
				case Interactables.Item:
					StartItemDialogue((Item) entity);
					break;
				case Interactables.NPC:
					StartDialogue((NPC) entity);
					break;
				case Interactables.Alert:
					StartSystemAlert(sysAnnounce);
					break;
				case Interactables.Diary:
					StartDiaryDialogue((Diary)entity);
					break;
				default: break;
			}
		}

		private void PrepareDialogue(Interactables interactable) {
			// Don't cut player movement if it is an alert
			if (interactable != Interactables.Alert) {
				_player.GetComponent<PlayerController>().enabled = false;
			}

			if (interactable != Interactables.NPC) {
				ToggleNextButton(false);
			}
				IsInteracting = true;
			_dialogueUI.DialogueText.text = "";
			_dialogueUI.Animator.SetBool("IsOpen", true);

		}

		public void SelectPrompt(int buttonIndex) {
			foreach (Button button in _dialogueUI.Buttons) {
				button.gameObject.SetActive(false);
			}
			_promptSelection = buttonIndex;
			_promptSelected = true;
		}

		public void ToggleNextButton(bool toggle) {
			_dialogueUI.NextButton.enabled = toggle;
			if (toggle) {
				_dialogueUI.NextButton.GetComponentInChildren<Text>().text = "Continue";
			} 
		}

		public void StartSystemAlert(Queue<(string, string)> sysAnnounce) {
			_sentences = sysAnnounce;
			DisplayNextSentence();
		}

		public void StartDiaryDialogue(Diary diary) {
			_sentences = diary.DescriptionQueue;
			DisplayNextSentence();
		}

		public void StartAnnouncement() {
			Awake();
			//_player.GetComponent<PlayerController>().enabled = true;
			DialogueDataManager.Initialize(DataType.SystemAnnouncement, 
			Constants.SystemAnnouncement, GameManager.SabotageId);
			_sentences = DialogueDataManager.GetAnnouncement();
			_dialogueUI.Animator.SetBool("IsOpen", true); 
            DisplayNextSentence();
        }

		public void StartItemDialogue(Item item) {
			_sentences = item.DescriptionQueue;
			DisplayNextSentence();
		}

		public void StartDialogue(NPC npc) {

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
			InitializePrompts();
			LoadAndDisplayPrompts();
			StartCoroutine(WaitForUserPrompt(npc));
            // Create Dialogue Object
        }

		public void ContinueNPCDialogue(NPC item) {
			ToggleNextButton(true);
			CreateDialogue(item);
			SimulateDialogue();
		}

		public void InitializePrompts() {
			for (int i = 0; i <_dialogueUI.Buttons.Length; i ++) {
				Button button =_dialogueUI.Buttons[i];
				int buttonIndex = i;
				button.onClick.AddListener(() => SelectPrompt(buttonIndex));
			}
		}

		public void LoadAndDisplayPrompts() {
			List<string> prompts = DialogueDataManager.GetPrompts();
			for (int i = 0;  i <_dialogueUI.Buttons.Length; i++) {
				if (i >= prompts.Count) {
					_dialogueUI.Buttons[i].gameObject.SetActive(false);
					continue;
				}
				_dialogueUI.Buttons[i].gameObject.SetActive(true);
				_dialogueUI.Buttons[i].GetComponentInChildren<Text>().text = prompts[i];
			}
		}

		IEnumerator WaitForUserPrompt(NPC character) {
			while (!_promptSelected) {
				yield return null;
			}
			yield return new WaitForSeconds(0.1f);
			_promptSelected = false;
			ContinueNPCDialogue(character);
		}

		public void CreateDialogue(InteractableObject entity) {
			_dialogue = new Dialogue();
			_dialogue.Name = entity.name;
			_dialogue.Sentences = DialogueDataManager.GetDialogue(_promptSelection);
		}

		/// <summary>
		/// ASK ABOUT THIS
		/// </summary>
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

			// If dialogue has ended
			if (_sentences.Count == 0) {
				EndDialogue();
				return;
			}

			(string, string) sentence = _sentences.Dequeue();

			StopAllCoroutines();
			StartCoroutine(TypeSentence(sentence));
		}

		IEnumerator TypeSentence((string, string) sentence) {
			_dialogueUI.NameText.text = sentence.Item1;
			_dialogueUI.DialogueText.text = "";

			foreach (char letter in sentence.Item2) { // may need to insert here:   .ToCharArray()
				_dialogueUI.DialogueText.text += letter;
				yield return new WaitForSeconds(_sentenceSpeed);
			}
		}

		void EndDialogue() {
			_dialogueUI.Animator.SetBool("IsOpen", false);
			_player.GetComponent<PlayerController>().enabled = true;
			IsInteracting = false;
		}
	}

	public enum Interactables {
		Item,
		NPC,
		Alert,
		Diary
	}
}
