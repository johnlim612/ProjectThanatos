using UnityEngine;

public class NPC : MonoBehaviour {
    public string Name { get { return _name; } }

    public bool HasBeenSpokenTo { get; }

    [SerializeField] private string _name;

    private int _countCharDialogue; // Incremented whenever character-specific dialogue is revealed
    private bool _hasBeenSpokenTo;  // If character-specific dialogue has been accessed that day

    private void Awake() {
        _countCharDialogue = 0;
        _hasBeenSpokenTo = false;
    }

    /// <summary>
    /// DialogueManager triggers the dialogue when the NPC is interacted with.
    /// </summary>
    public void TriggerDialogue() {
        int? charDialogueKey;

        if (_hasBeenSpokenTo) {
            charDialogueKey = null;
        } else {
            charDialogueKey = _countCharDialogue;
        }

        DialogueManager.StartDialogue(_name, charDialogueKey);
    }
}
