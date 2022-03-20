using System.Collections.Generic;
using UnityEngine;

public class SystemAnnouncement: MonoBehaviour {

    private static Queue<(string, string)> _descriptionQueue = new Queue<(string, string)>();

    private static Queue<(string, string)> DescriptionQueue {
        get {
            return _descriptionQueue;
        }
    }

    private static void AddToQueue(string[] sentences) {
        foreach (string sentence in sentences) {
            _descriptionQueue.Enqueue(("Player", sentence));
        }
    }

    public static void Announce(string[] description) {
        AddToQueue(description);
        FindObjectOfType<UI.UIDialogueManager>().InitializeDialogue(UI.EntityType.Announcement , sysAlert: _descriptionQueue);
    }
}