using System.Collections.Generic;
using UnityEngine;

public class Item : InteractableObject {
    
    [SerializeField] private string[] _descriptions;
    private Queue<(string, string)> _descriptionQueue = new Queue<(string, string)>();

	void Start() {
        //AddToQueue(_descriptions);
	}
	public override void InteractObject() {
        Interact();
    }

    public Queue<(string, string)> DescriptionQueue {
        get { return _descriptionQueue; }
    }

    public void AddToQueue(string[] sentences) {
        foreach (string sentence in sentences) {
            _descriptionQueue.Enqueue(("Player", sentence));
		}
	}

    private void Interact() {
        AddToQueue(_descriptions);
        FindObjectOfType<UI.UIDialogueManager>().StartItemDialogue(this);
    }
}
