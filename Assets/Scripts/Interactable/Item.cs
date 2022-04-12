using System.Collections.Generic;
using UnityEngine;

public class Item : InteractableObject {
    
    [SerializeField] private string[] _descriptions;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _glowRange = 7;
    private Queue<(string, string)> _descriptionQueue = new Queue<(string, string)>();
    private cakeslice.Outline _outline;


    [HideInInspector]
    public bool Obtained { get; set; }

	void Awake() {
        Obtained = false;
        _player = GameObject.Find(Constants.PlayerKey);
        _outline = gameObject.GetComponent<cakeslice.Outline>();
    }

	void Start() {
        //AddToQueue(_descriptions);
	}

	public override void InteractObject() {
        Interact();
    }

    public Queue<(string, string)> DescriptionQueue {
        get { return _descriptionQueue; }
    }

	void Update() {
        if (_outline != null) {
            RegulateGlow();
		}
	}
	public void RegulateGlow() {
        float dis = Vector3.Distance(transform.position, _player.transform.position);
        if (dis < _glowRange) {
            _outline.enabled = true; 
        } else {
            _outline.enabled = false;
		}
    }

    public void AddToQueue(string[] sentences) {
        foreach (string sentence in sentences) {
            _descriptionQueue.Enqueue(("Player", sentence));
		}
	}

    private void Interact() {
        AddToQueue(_descriptions);
        print("Item was Detected!!!!!");
        FindObjectOfType<UI.UIDialogueManager>().InitializeDialogue(UI.EntityType.Item, this);
        Obtained = true;
        gameObject.SetActive(false);
    }
}
