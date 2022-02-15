using System.Collections.Generic;
using UnityEngine;

public class SystemAnnouncement: MonoBehaviour {

    [SerializeField] private string[] _descriptions;
    private Queue<(string, string)> _descriptionQueue = new Queue<(string, string)>();

    public Queue<(string, string)> DescriptionQueue {
        get {
            return _descriptionQueue;
        }
    }

    public void AddToQueue(string[] sentences) {
        foreach (string sentence in sentences) {
            _descriptionQueue.Enqueue(("Player", sentence));
        }
    }

    private void Announce() {
        AddToQueue(_descriptions);
        FindObjectOfType<UI.UIDialogueManager>().StartSystemAlert(this);
    }
}