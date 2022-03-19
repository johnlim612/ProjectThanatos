using System;
using System.Collections.Generic;

public class NPC : InteractableObject {
    public int CountCharDialogue { get { return _countCharDialogue; } }

    public bool HasBeenSpokenTo { 
        get { return _hasBeenSpokenTo; }
        set { _hasBeenSpokenTo = value; }
    }

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
        // FindObjectOfType<UI.UIDialogueManager>().StartDialogue(this);
        FindObjectOfType<UI.UIDialogueManager>().InitializeDialogue(UI.EntityType.NPC, this);

    }

    public void UpdateCharDialogueProgress() {
        _hasBeenSpokenTo = true;
        ++_countCharDialogue;
    }
}
