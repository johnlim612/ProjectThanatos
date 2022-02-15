using System.Collections.Generic;
using UnityEngine;

public class InteractableSabotage : InteractableObject {
    [SerializeField] private string[] _descriptions;
    [SerializeField] private int _sabotageID;

    public int SabotageID {
        get {
            return _sabotageID;
        }
    }

    public override void InteractObject() {
        print("Sabotage Repaired");
        GameManager.ClearSabotage();
        Interact();
    }

    private void Interact() {
        SystemAnnouncement.Announce(_descriptions);
    }
}
