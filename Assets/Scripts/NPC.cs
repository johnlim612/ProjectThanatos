public class NPC : InteractableObject {
    public int CountCharDialogue { get { return _countCharDialogue; } }

    public bool HasBeenSpokenTo { get { return _hasBeenSpokenTo; } }

    private int _countCharDialogue; // Incremented whenever character-specific dialogue is revealed
    private bool _hasBeenSpokenTo;  // If character-specific dialogue has been accessed that day

    private void Awake() {
        _countCharDialogue = 0;
        _hasBeenSpokenTo = false;
    }

    /// <summary>
    /// DialogueManager triggers the dialogue when the NPC is interacted with.
    /// </summary>
    public override void InteractObject() {
        int? charDialogueKey;

        if (_hasBeenSpokenTo) {
            charDialogueKey = null;
        } else {
            charDialogueKey = _countCharDialogue;
        }

        FindObjectOfType<UI.UIDialogueManager>().StartDialogue(this);
    }
}
