using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager Instance { get { return _instance;  } }

    private static DialogueManager _instance;

    private static string _baseFilePath = "Dialogue/";
    private static string[] _prompts;
    private static string _greeting;

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

        FindRelevantDialogue(data);
    }

    private static JObject LoadData(string fileName) {
        string filePath = _baseFilePath + fileName;

        var jsonDialogueFile = Resources.Load<TextAsset>(filePath);
        
        return (JObject) JsonConvert.DeserializeObject(jsonDialogueFile.text);
    }

    private static void FindRelevantDialogue(JObject data, int? charDialogueId = null) {
        int id = GameManager.SabotageId;
    }

    private static void SortQueue() {

    }

    private static string[] GetPrompts() {
        return _prompts;
    }
}
