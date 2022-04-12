using System.Collections.Generic;
using UnityEngine;

public class Item : InteractableObject {
    
    [SerializeField] private string[] _descriptions;
    private Queue<(string, string)> _descriptionQueue = new Queue<(string, string)>();

    [HideInInspector]
    public bool Obtained { get; set; }

	void Awake() {
        Obtained = false;
    }

	void Start() {
        //AddToQueue(_descriptions);
	}

	public override void InteractObject() {
        print("Item was Detected!!!!!");
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
        FindObjectOfType<UI.UIDialogueManager>().InitializeDialogue(UI.EntityType.Item, this);
        Obtained = true;
        gameObject.SetActive(false);
    }
}
