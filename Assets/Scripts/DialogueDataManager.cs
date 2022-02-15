using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public struct DialogueReference {
    public string EventType;    // E.g. "sabotage", "random", etc.
    public string EventKey;     // E.g. if random event = "coffee", else an int "1"

    public DialogueReference(string eventType, string eventKey) {
        EventType = eventType;
        EventKey = eventKey;
    }
}

public class DialogueDataManager : MonoBehaviour {
    public static DialogueDataManager Instance { get { return _instance;  } }

    private static DialogueDataManager _instance;   // Singleton instance of the class.

    private static string _baseFilePath = "Dialogue/";
    private static List<string> _prompts;   // Players' prompts to start different convos w/ NPCs
    private static string _greeting;    // First thing NPC displays when interacted with.
    private static Queue<(string, string)> _systemAnnouncements; // Alert related to day's sabotage

    // Indices of dialogues correspond to _prompts[]
    private static List<DialogueReference> _dialogues;

    // Contains specific raw dialgoue data in JSON format
    private static JToken _sabotageData;    // The sabotage-specific dialogue data
    private static (string, JToken)[] _randomEventData; // Array of Tuples of events' key/values
    private static JToken _characterData;   // The character-specific dialogue data
    private static string _dataRefName; // Name of the item/character referenced in the JSON data

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        _prompts = new List<string>();
        _dialogues = new List<DialogueReference>();
    }

    /// <summary>
    /// Load the JSON file containing the dialogue.
    /// </summary>
    /// <param name="dataType">Enum specifying which type of data should be loaded</param>
    /// <param name="fileName">The NPC whose JSON file will be loaded</param>
    /// <param name="dialogueId">*Optional* Key for character-specific dialogue</param>
    public static void Initialize(DataType dataType, string fileName, int? dialogueId = null) {
        // Remove whitespace from characters' names before searching for the file.
        JObject data = LoadData(fileName.Replace(" ", ""));
        _dataRefName = fileName;

        if (data == null) {
            Debug.LogError($"Unable to locate file with name {fileName.Replace(" ", "")}");
            return;
        }

        switch (dataType) {
            case DataType.CharacterDialogue:
                FindRelevantDialogue(data, dialogueId);
                FindInitialPrompts();
                break;
            case DataType.SystemAnnouncement:
                FindSystemAnnouncement(data, dialogueId);
                break;
            default:
                Debug.LogError($"Invalid DataType. Data Type ${dataType} was not recognized.");
                break;
        }
    }

    /// <summary>
    /// Locate and access the JSON file that contains all of a character's dialogue.
    /// </summary>
    /// <param name="fileName">Name of file; should be the character's name without spaces.</param>
    /// <returns>Deserialized JSON as a JObject.</returns>
    private static JObject LoadData(string fileName) {
        string filePath = _baseFilePath + fileName;

        var jsonDialogueFile = Resources.Load<TextAsset>(filePath);
        
        return (JObject) JsonConvert.DeserializeObject(jsonDialogueFile.text);
    }

    private static void FindRelevantDialogue(JObject data, int? charDialogueId) {
        string sabotageId = GameManager.SabotageId.ToString();
        _sabotageData = data[Constants.SabotageDialogueKey][sabotageId];

        // Get dialogue related to all ongoing random events.
        if (GameManager.RandomEventIds.Count != 0) {
            _randomEventData = new (string, JToken)[GameManager.RandomEventIds.Count];
            string randEventId;

            for (int i = 0; i < GameManager.RandomEventIds.Count; i++) {
                randEventId = GameManager.RandomEventIds[i];
                _randomEventData[i] = (randEventId, data[Constants.RandomEventDialogueKey][randEventId]);
            }
        }

        // Get character-specific dialogue if it's available.
        if (charDialogueId != null) {
            _characterData = data[Constants.CharacterDialogueKey][charDialogueId.ToString()];
        }

        _greeting = (string) _sabotageData["greeting"];
    }

    /// <summary>
    /// Search stored data for the prompts to start different dialogues.
    /// Store references to the dialogue each prompt belongs to by making a new DialogueReference.
    /// </summary>
    private static void FindInitialPrompts() {
        string promptKey = Constants.DialoguePromptKey;

        _prompts.Add(_sabotageData[promptKey]["1"].ToString());
        _dialogues.Add(new DialogueReference(Constants.SabotageDialogueKey, "1"));

        if (_randomEventData.Length > 0) {
            foreach ((string, JToken) randEvent in _randomEventData) {
                _prompts.Add(randEvent.Item2[promptKey]["1"].ToString());
                _dialogues.Add(new DialogueReference(Constants.RandomEventDialogueKey, 
                    randEvent.Item1));
            }
        }

        if (_characterData != null) {
            _prompts.Add(_characterData[Constants.DialoguePromptKey]["1"].ToString());
            _dialogues.Add(new DialogueReference(Constants.CharacterDialogueKey, "1"));
        }
    }

    /// <summary>
    /// Sorts the sentences in the correct order, with "prompt" being given
    /// priority over "sentence".
    /// </summary>
    /// <param name="data"></param>
    /// <returns>Sorted Queue</returns>
    private static Queue<(string, string)> SortQueue(JToken data) {
        Queue<(string, string)> dialogue = new Queue<(string, string)>();

        List<(int, string)> prompts = new List<(int, string)>(); // Said by Player
        List<(int, string)> replies = new List<(int, string)>(); // Said by NPC

        foreach (JProperty prompt in data[Constants.DialoguePromptKey]) {
            // sentence = { "1": "hi" }
            prompts.Add((Int32.Parse(prompt.Name), prompt.Value.ToString()));
        }

        foreach (JProperty sentence in data[Constants.DialogueNPCSentenceKey]) {
            // sentence = { "1": "hi" }
            replies.Add((Int32.Parse(sentence.Name), sentence.Value.ToString()));
        }

        // Compare value of keys in the two temp lists and sort into queue
        int i = 0;
        int j = 0;

        while (i < prompts.Count && j < replies.Count) {
            if (prompts[i].Item1 > replies[j].Item1) {
                dialogue.Enqueue((_dataRefName, replies[j].Item2));
                j++;
            } else {
                dialogue.Enqueue(("player", prompts[i].Item2));
                i++;
            }
        }
        // Add the remaining queue
        if (i == prompts.Count) {
            for (int k = j; k < replies.Count; k++) {
                dialogue.Enqueue((_dataRefName, replies[k].Item2));
            }
        } else {
            for (int l = i; l < prompts.Count; l++) {
                dialogue.Enqueue(("player", prompts[l].Item2));
            }
        }

        return dialogue;
    }

    /// <summary>
    /// Determines which dialogue was selected and sends the correct data to be sorted.
    /// Returns a sorted Queue of tuples, which are ordered in the correct
    /// flow of the player-to-NPC conversation.
    /// </summary>
    /// <param name="selectedDialogue">int corresponds to index of selected prompt</param>
    /// <returns>A Queue of tuples: ("character name", "sentence")</returns>
    public static Queue<(string, string)> GetDialogue(int selectedDialogue) {
        DialogueReference dRef = _dialogues[selectedDialogue];
        Queue<(string, string)> dialogue = new Queue<(string, string)>();

        switch (dRef.EventType) {
            case Constants.SabotageDialogueKey:
                dialogue = SortQueue(_sabotageData);
                break;
            case Constants.RandomEventDialogueKey:
                foreach ((string, JToken) randEvent in _randomEventData) {
                    if (dRef.EventKey.Equals(randEvent.Item1)) {
                        dialogue = SortQueue(randEvent.Item2);
                        break;
                    }
                }
                break;
            case Constants.CharacterDialogueKey:
                dialogue = SortQueue(_characterData);
                break;
            default:
                return null;
        }
        return dialogue;
    }

    public static string GetGreeting() {
        return _greeting;
    }

    public static List<string> GetPrompts() {
        return _prompts;
    }

    // ------------------------------- SYSTEM ANNOUNCEMENTS ------------------------------- //

    public static void FindSystemAnnouncement(JObject data, int? alertId) {
        if (alertId == null) {
            return;
        }

        JToken rawAlertData = data[Constants.SabotageDialogueKey][alertId.ToString()];

        if (rawAlertData != null) {
            SortAnnouncementQueue(rawAlertData);
        }
    }

    private static void SortAnnouncementQueue(JToken data) {
        _systemAnnouncements = new Queue<(string, string)>();
        JToken alert;
        JToken message;

        alert = data[Constants.SystemAlertKey];
        message = data[Constants.SystemMessageKey];

        if (alert != null) {
            _systemAnnouncements.Enqueue((Constants.SystemAlertKey, alert.ToString()));
        }

        if (message != null) {
            _systemAnnouncements.Enqueue((Constants.SystemMessageKey, message.ToString()));
        }
    }

    public static Queue<(string, string)> GetAnnouncement() {
        return _systemAnnouncements;
    }
}

public enum DataType {
    SystemAnnouncement,
    CharacterDialogue,
}
