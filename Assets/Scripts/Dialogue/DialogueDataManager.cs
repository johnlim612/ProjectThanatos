using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UI; // UIDialogueManager Namespace containing EnityType Enum

public class DialogueDataManager : MonoBehaviour {
    public struct DialogueReference {
        public string EventType;    // E.g. "sabotage", "random", etc.
        public string EventKey;     // E.g. if random event = "coffee", else an int "1"

        public DialogueReference(string eventType, string eventKey) {
            EventType = eventType;
            EventKey = eventKey;
        }
    }
    public static DialogueDataManager Instance { get { return _instance;  } }

    private static DialogueDataManager _instance;   // Singleton instance of the class.

    // Contains parsed data:
    private string _baseFilePath = "Dialogue/";

    private List<string> _prompts;   // Players' prompts to start different convos w/ NPCs
    private string _greeting;    // First thing NPC displays when interacted with.

    private Queue<(string, string)> _systemAnnouncements; // Alert related to day's sabotage
    private Queue<(string, string)> _completedSabotageAlert; // Alert when the day's sabotage is fixed

    private string _diaryEntry; // The player's end-of-day diary entry.
    private List<string> _questLog; // The list of objectives for the day's sabotage.

    // Indices of dialogues correspond to _prompts[]
    private List<DialogueReference> _dialogues;

    // Contains specific raw dialgoue data in JSON format:
    private JToken _sabotageData;    // The sabotage-specific dialogue data
    private (string, JToken)[] _randomEventData; // Array of Tuples of events' key/values
    private JToken _characterData;   // The character-specific dialogue data
    private string _dataRefName; // Name of the item/character referenced in the JSON data
    private string _dataDayKey;

    public void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        _prompts = new List<string>();
        _dialogues = new List<DialogueReference>();
        _questLog = new List<string>();
        _diaryEntry = "";
    }

    /// <summary>
    /// Load the JSON file containing the dialogue.
    /// </summary>
    /// <param name="dataType">Enum specifying which type of data should be loaded</param>
    /// <param name="fileName">The NPC whose JSON file will be loaded</param>
    /// <param name="id">*Optional* Key for character-specific dialogue</param>
    public void Initialize(EntityType dataType, string fileName, int? charBackstoryId = null) {
        // Ensure any previously stored data is cleared
        ResetData();

        // Remove whitespace from characters' names before searching for the file.
        JObject data = LoadData(fileName.Replace(" ", ""));
        _dataRefName = fileName;
        _dataDayKey = "D" + GameManager.Instance.Day.ToString();
        print(data);
        if (data == null) {
            Debug.LogError($"Unable to locate file with name {fileName.Replace(" ", "")}");
            return;
        }

        switch (dataType) {
            case EntityType.NPC:
                ParseNpcData(data, charBackstoryId);
                break;
            case EntityType.Alert:
                ParseSystemAlert(data, _dataDayKey);
                break;
            case EntityType.Diary:
                ParseDiaryEntry(data, _dataDayKey);
                break;
            default:
                Debug.LogError($"Invalid DataType. Data Type {dataType} was not recognized.");
                break;
        }
    }

    /// <summary>
    /// Locate and access the JSON file that contains all of a character's dialogue.
    /// </summary>
    /// <param name="fileName">Name of file; should be the character's name without spaces.</param>
    /// <returns>Deserialized JSON as a JObject.</returns>
    private JObject LoadData(string fileName) {
        string filePath = _baseFilePath + fileName;
        var jsonDialogueFile = Resources.Load<TextAsset>(filePath);

        return (JObject) JsonConvert.DeserializeObject(jsonDialogueFile.text);
    }

    private void ParseNpcData(JObject data, int? charBackstoryId) {
        _greeting = data[Constants.GreetingKey][_dataDayKey]["1"][Constants.NpcKey].ToString();

        _sabotageData = data[Constants.QuestLogKey][_dataDayKey];

        // Get dialogue related to all ongoing random events.
        //if (GameManager.RandomEventIds.Count != 0) {
        //    _randomEventData = new (string, JToken)[GameManager.RandomEventIds.Count];
        //    string randEventId;

        //    for (int i = 0; i < GameManager.RandomEventIds.Count; i++) {
        //        randEventId = GameManager.RandomEventIds[i];
        //        _randomEventData[i] = (randEventId, data[Constants.RandomEventDialogueKey][randEventId]);
        //    }
        //}

        // Get character-specific dialogue if it's available.
        if (charBackstoryId != null && charBackstoryId != 0) {
            string backstoryId = "D" + charBackstoryId.ToString();
            _characterData = data[Constants.CharacterDialogueKey][backstoryId];
        }

        FindInitialPrompts();
    }

    /// <summary>
    /// Search stored data for the prompts to start different dialogues.
    /// Store references to the dialogue each prompt belongs to by making a new DialogueReference.
    /// </summary>
    private void FindInitialPrompts() {
        _prompts.Add(_sabotageData["1"][Constants.PlayerKey].ToString());
        _dialogues.Add(new DialogueReference(Constants.SabotageDialogueKey, "1"));

        //if (_randomEventData != null && _randomEventData.Length > 0) {
        //    foreach ((string, JToken) randEvent in _randomEventData) {
        //        _prompts.Add(randEvent.Item2[_dataDayKey]["1"].ToString());
        //        _dialogues.Add(new DialogueReference(Constants.RandomEventDialogueKey, 
        //            randEvent.Item1));
        //    }
        //}

        if (_characterData != null) {
            _prompts.Add(_characterData["1"][Constants.PlayerKey].ToString());
            _dialogues.Add(new DialogueReference(Constants.CharacterDialogueKey, "1"));
        }
    }

    /// <summary>
    /// Sorts the sentences in the correct speaking order.
    /// </summary>
    /// <param name="data"></param>
    /// <returns>Sorted Queue</returns>
    private Queue<(string, string)> QueueNpcDialogue(JToken data) {
        Queue<(string, string)> dialogue = new Queue<(string, string)>();
        string speaker = "";
        string sentence = "";

        foreach (JProperty property in data) {
            // property = { 1: { "speaker": "sentence" } }
            if (property == null) {
                break;
            }

            if (property.Value[Constants.PlayerKey] != null) {
                speaker = Constants.PlayerKey;
                sentence = property.Value[Constants.PlayerKey].ToString();
            } else if (property.Value[Constants.NpcKey] != null) {
                speaker = _dataRefName;
                sentence = property.Value[Constants.NpcKey].ToString();
            }

            dialogue.Enqueue((speaker, sentence));
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
    public Queue<(string, string)> GetDialogue(int selectedDialogue) {
        DialogueReference dRef = _dialogues[selectedDialogue];
        Queue<(string, string)> dialogue = new Queue<(string, string)>();

        switch (dRef.EventType) {
            case Constants.SabotageDialogueKey:
                dialogue = QueueNpcDialogue(_sabotageData);
                break;
            case Constants.RandomEventDialogueKey:
                foreach ((string, JToken) randEvent in _randomEventData) {
                    if (dRef.EventKey.Equals(randEvent.Item1)) {
                        dialogue = QueueNpcDialogue(randEvent.Item2);
                        break;
                    }
                }
                break;
            case Constants.CharacterDialogueKey:
                dialogue = QueueNpcDialogue(_characterData);
                break;
            default:
                return null;
        }
        return dialogue;
    }

    public string GetGreeting() {
        return _greeting;
    }

    public List<string> GetPrompts() {
        return _prompts;
    }

    private void ResetData() {
        _prompts = new List<string>();
        _dialogues = new List<DialogueReference>();
        _diaryEntry = "";
        _questLog = new List<string>();
    }

    // ---------------------------- SHARED BY SABOTAGE LAYER IN JSON --------------------------- //

    private void FindSabotageRelatedData(EntityType dataType, JObject data, int? id) {
        if (id == null) {
            Debug.LogError("id is null");
            return;
        }

        JToken rawData = data[Constants.SabotageDialogueKey][id.ToString()];

        if (rawData == null) {
            Debug.LogError("rawData is null");
            return;
        }

        switch (dataType) {
            case EntityType.Diary:
                ParseTabletData(rawData);
                break;
        }
    }

    // ------------------------------- SYSTEM ANNOUNCEMENTS ------------------------------- //
    private void ParseSystemAlert(JObject data, string key) {
        JToken sabotageData = data[Constants.SabotageDialogueKey][key];
        JToken sabotageCompletedData = data[Constants.SabotageCompletedKey][key];

        if (sabotageData == null || sabotageCompletedData == null) {
            Debug.LogError($"System Alert data is null.\n'Sabotage': " +
                $"{sabotageData}, 'Fix-Complete': {sabotageCompletedData}");
            return;
        }

        _systemAnnouncements = QueueAlertMessages(sabotageData);
        _completedSabotageAlert = QueueAlertMessages(sabotageCompletedData);

    }

    private Queue<(string, string)> QueueAlertMessages(JToken data) {
        Queue<(string, string)> queue = new Queue<(string, string)>();
        string speaker = "";
        string sentence = "";

        foreach (JProperty property in data) {
            // property = { 1: { "speaker": "sentence" } }
            if (property.Value[Constants.PlayerKey] != null) {
                speaker = Constants.PlayerKey;
                sentence = property.Value[Constants.PlayerKey].ToString();
            } else if (property.Value[Constants.SystemAlertKey] != null) {
                speaker = Constants.SystemAlertKey;
                sentence = property.Value[Constants.SystemAlertKey].ToString();
            }

            queue.Enqueue((speaker, sentence));
        }

        return queue;
    }

    public Queue<(string, string)> GetAnnouncement() {
        return _systemAnnouncements;
    }

    public Queue<(string, string)> GetFixedSabotageMsg() {
        return _completedSabotageAlert;
    }

    // ------------------------------- TABLET - DIARY & QUEST LOG ------------------------------- //
    private void ParseDiaryEntry(JObject data, string dayKey) {
        if (dayKey.Equals(Constants.TheEnd)) {
            // check which ending was selected via GameManager or TabletManager???
            return;
        }

        foreach (JProperty entry in data[dayKey]) {
            // log = { "1": "hi" }
            _diaryEntry += entry.Value.ToString();
        }
    }

        private void ParseTabletData(JToken data) {
        _diaryEntry = data[Constants.DiaryKey].ToString();
        // Parse and store quest log data
        foreach (JProperty log in data[Constants.QuestLogKey]) {
            // log = { "1": "hi" }
            _questLog.Add(log.Value.ToString());
        }
    }

    public string GetDiaryEntry() {
        return _diaryEntry;
    }

    //public List<string> GetQuestLog() {
    //    return _questLog;
    //}
}
