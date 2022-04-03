using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class UIDialogueManager: MonoBehaviour {
		public DialogueUI DialogueUI;

		public static UIDialogueManager Instance { get {
				return _instance; } }
		private static UIDialogueManager _instance;
		public bool IsInteracting = false;

		// Temp Dialogue queue holder
		private Queue<(string, string)> _sentences;

		// Waits for user to select prompt
		private bool _promptSelected = false;
		private bool _questSelected = false;

		private int _promptSelection = 0;
		private (string, string) _tempSentence;
		private GameObject _player;
		private InteractableObject _entity;
		private EntityType _entityType;
		private Dialogue _dialogue;

		private const float _sentenceSpeed = 0.02f;

		// Start is called before the first frame update
		void Awake() {
			if (_instance != null && _instance != this) {
				Destroy(this.gameObject);
			} else {
				_instance = this;
				DontDestroyOnLoad(this.gameObject);
			}

			_sentences = new Queue<(string, string)>();
			_player = GameObject.Find(Constants.PlayerKey);
			_tempSentence = (null, null);
			_entityType = EntityType.NPC;
			_entity = null;
		}

		// add function on prompt buttons
		void Start() {
			InitializePrompts();
		}

		public void InitializePrompts() {
			for (int i = 0; i < DialogueUI.Buttons.Length; i++) {
				Button button = DialogueUI.Buttons[i];
				int buttonIndex = i;
				button.onClick.AddListener(() => SelectPrompt(buttonIndex));
			}
		}

		public void InitializeDialogue(EntityType interactable, InteractableObject entity = null, Queue <(string, string)> sysAlert = null) {
			// If entity is not registered

			this._entity = entity;
			if (!Enum.IsDefined(typeof(EntityType), interactable)) {
				Debug.Log("Entity not defined");
				return;
				// item not recognized
			}

			_entityType = interactable;
			PrepareDialogue();

			switch (interactable) {
				case EntityType.Item:
					StartItemDialogue((Item) entity);
					break;
				case EntityType.NPC:
					DialogueUI.UpdateImage(entity.name);
					StartDialogue((NPC) entity);
					break;
				case EntityType.Alert:
					StartSystemAlert();
					break;
				case EntityType.Diary:
					StartDiaryDialogue((Bed) entity);
					break;
				case EntityType.Player:
					_questSelected = true;
					StartMonologue();
					break;
				default: 
					break;
			}
		}

		private void PrepareDialogue() {
			// Don't cut player movement if it is an alert
			if (_entityType != EntityType.Alert) {
				_player.GetComponent<PlayerController>().enabled = false;
			}

			if (_entityType == EntityType.NPC) {
				ToggleNextButton(false);
			}

			// Close Tablet if it's already open
			TabletManager.Instance.ToggleTabletState(false);

            IsInteracting = true;
			DialogueUI.DialogueText.text = "";
			DialogueUI.Animator.SetBool("IsOpen", true);
			Cursor.lockState = CursorLockMode.None;
		}

		public void StartSystemAlert() {
			DialogueDataManager.Instance.Initialize(EntityType.Alert, Constants.SystemAnnouncement,
										GameManager.Instance.Day);
			bool isActive = GameManager.Instance.CurrentSabotage.IsActive;
			_sentences = (isActive) ? DialogueDataManager.Instance.GetAnnouncement() :
				DialogueDataManager.Instance.GetFixedSabotageMsg();
			
			PrepareDialogue();
			DisplayNextAction();
		}

		public void StartDiaryDialogue(Bed bed) {
			_sentences = bed.DescriptionQueue;
			DisplayNextAction();
		}

		public void StartItemDialogue(Item item) {
			_sentences = item.DescriptionQueue;
			DisplayNextAction();
		}

		public void StartMonologue() {
			DialogueDataManager.Instance.Initialize(EntityType.Player, Constants.SystemAnnouncement,
													GameManager.Instance.SabotageId);
			_sentences = DialogueDataManager.Instance.GetAnnouncement();
			DisplayNextAction();
		}

		public void ToggleNextButton(bool toggle) {
			DialogueUI.NextButton.enabled = toggle;
			if (toggle) {
				DialogueUI.NextButton.GetComponentInChildren<Text>().text = "Continue";
			} else {
				DialogueUI.NextButton.GetComponentInChildren<Text>().text = "";
			}
		}

		public void StartDialogue(NPC npc) {

			// CHECK IF NPC HAS QUEST AVAILABLE AND USE/STORE/MAKE TRU THE PROMPT QUEST
			
			if (npc.HasBeenSpokenTo) {
                DialogueDataManager.Instance.Initialize(EntityType.NPC, npc.gameObject.name);
			} else {
                DialogueDataManager.Instance.Initialize(EntityType.NPC, npc.gameObject.name, 
				npc.CountCharDialogue);
				npc.UpdateCharDialogueProgress();
			}

			// Starts greeting
			StopAllCoroutines();
			StartCoroutine(TypeSentence((npc.gameObject.name, DialogueDataManager.Instance.GetGreeting())));

			LoadAndDisplayPrompts();
			StartCoroutine(WaitForUserFirstPrompt(npc));
        }

		public void LoadAndDisplayPrompts(string sentence = null) {
			List<string> prompts;
			
			if (sentence is null) {
				prompts = DialogueDataManager.Instance.GetPrompts();
			} else {
				prompts = new List<string> { sentence };
			}

			for (int i = 0; i < DialogueUI.Buttons.Length; i++) {
				if (i >= prompts.Count) {
					DialogueUI.Buttons[i].gameObject.SetActive(false);
					continue;
				}
				DialogueUI.Buttons[i].gameObject.SetActive(true);
				DialogueUI.Buttons[i].GetComponentInChildren<Text>().text = prompts[i];
			}
		}

		public void SelectPrompt(int buttonIndex) {
			foreach (Button button in DialogueUI.Buttons) {
				button.gameObject.SetActive(false);
			}

			_promptSelection = buttonIndex;
			_promptSelected = true;

			if (_entityType is EntityType.NPC) { // && _entity.ActiveQuest is true && _promptSelection is Constants.QuestPrompt
				_questSelected = true;
			}
		}

		public void ContinueNPCDialogue(NPC npc) {
			ToggleNextButton(true);
			CreateDialogue(npc);
			_sentences = _dialogue.Sentences;
			DisplayNextAction(true);
		}

		IEnumerator WaitForUserFirstPrompt(NPC character) {
			while (!_promptSelected) {
				yield return new WaitForSeconds(0.1f);
			}
			_promptSelected = false;
			ContinueNPCDialogue(character);
		}

		IEnumerator WaitForUserPrompt() {
			while (!_promptSelected) {
				yield return new WaitForSeconds(0.1f);
			}
			_promptSelected = false;
			RunSentenceCoroutines(_tempSentence);
		}

		public void CreateDialogue(InteractableObject entity) {
			_dialogue = new Dialogue();
			_dialogue.Name = entity.name;
			_dialogue.Sentences = DialogueDataManager.Instance.GetDialogue(_promptSelection);
		}
 
		public void DisplayNextAction(bool firstPrompt = false) {
			ToggleNextButton(false);
			
			if (_sentences.Count == 0) {
				EndDialogue();
				return;
			}

			_tempSentence = _sentences.Dequeue();

			// if its not the initial prompt and its a player go through prompt
			if (!firstPrompt && _tempSentence.Item1 == Constants.PlayerKey && _entityType == EntityType.NPC) {
				// For prompt
				LoadAndDisplayPrompts(_tempSentence.Item2);
				StartCoroutine(WaitForUserPrompt());
			} else {
				// For everything else including initial prompt
				RunSentenceCoroutines(_tempSentence);
			}
		}

		private void RunSentenceCoroutines((string, string) sentence) {
			StopAllCoroutines();
			StartCoroutine(TypeSentence(sentence));
			StartCoroutine(DelayNextButton());
		}

		public void UpdateNextSentence() {
			// If dialogue has ended
			if (_sentences.Count == 0) {
				EndDialogue();
				return;
			}
			_tempSentence = _sentences.Dequeue();
		}

		IEnumerator DelayNextButton() {
			yield return new WaitForSeconds(1);
			ToggleNextButton(true);
		}

		IEnumerator TypeSentence((string, string) sentence) {
			DialogueUI.NameText.text = sentence.Item1;
			DialogueUI.DialogueText.text = "";

			foreach (char letter in sentence.Item2) { 
				DialogueUI.DialogueText.text += letter;
				yield return new WaitForSeconds(_sentenceSpeed);
			}
		}

		void EndDialogue() {
			DialogueUI.Animator.SetBool("IsOpen", false);
			_player.GetComponent<PlayerController>().enabled = true;
			Cursor.lockState = CursorLockMode.Locked;
			IsInteracting = false;
			DialogueUI.ImageBox.enabled = false;

			if (_questSelected) {
				_questSelected = false;
				QuestManager.Instance.TriggerNext();
			}
		}
	}

	public enum EntityType {
		Item,
		NPC,
		Alert,
		Diary,
		Player,
		Room,
		Corpse,
		QuestLog
	}
}
