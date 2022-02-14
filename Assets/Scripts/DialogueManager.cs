using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager Instance { get { return _instance;  } }

    private static DialogueManager _instance;

    private static string _baseFilePath = "Dialogue/";
    private static string[] _prompts;
    private static string _greeting;

    private Dialogue sabotageDialogue = new Dialogue();
    private Dialogue randomDialogue = new Dialogue();
    private Dialogue characterDialogue = new Dialogue();

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    /// <summary>
    /// Load the JSON file containing the dialogue.
    /// </summary>
    /// <param name="npcName">The NPC whose JSON file will be loaded</param>
    /// <param name="charDialogueId">Key for character-specific dialogue</param>
    public static void StartDialogue(string npcName, int? charDialogueId) {
        // Remove whitespace from characters' names before searching for the file.
        JObject data = LoadData(npcName.Replace(" ", ""));

        FindRelevantDialogue(data, 1);  // Change 1 to charDialogueId after testing.
    }

    private static JObject LoadData(string fileName) {
        string filePath = _baseFilePath + fileName;

        var jsonDialogueFile = Resources.Load<TextAsset>(filePath);
        
        return (JObject) JsonConvert.DeserializeObject(jsonDialogueFile.text);
    }

    private static void FindRelevantDialogue(JObject data, int? charDialogueId = null) {
        string sabotageId = GameManager.SabotageId.ToString();
        JToken sabotageData = data[Constants.SabotageDialogueKey][sabotageId];
        JToken[] randomEventData = null;
        JToken characterData = null;

        // Get dialogue related to all ongoing random events.
        if (GameManager.RandomEventIds.Count != 0) {
            randomEventData = new JToken[GameManager.RandomEventIds.Count];
            string randEventId;

            for (int i = 0; i < GameManager.RandomEventIds.Count; i++) {
                randEventId = GameManager.RandomEventIds[i];
                randomEventData[i] = data[Constants.RandomEventDialogueKey][randEventId];
            }
        }

        // Get character-specific dialogue if it's available.
        if (charDialogueId != null) {
            characterData = data[Constants.CharacterDialogueKey][charDialogueId.ToString()];
        }

        _greeting = (string) sabotageData["greeting"];
    }

    private static void SortQueue() {

    }

    public static string GetGreeting() {
        return _greeting;
    }

    public static string[] GetPrompts() {
        return _prompts;
    }
}
