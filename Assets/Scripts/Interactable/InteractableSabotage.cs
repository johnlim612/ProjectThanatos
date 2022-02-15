using System.Collections.Generic;
using UnityEngine;

public class InteractableSabotage : InteractableObject {
    [SerializeField] private string[] _descriptions;
    [SerializeField] private int _sabotageID;
    private BoxCollider2D _box2D;
    private SpriteRenderer _spriteRend;

    private void Awake() {
        _box2D = GetComponent<BoxCollider2D>();
        _spriteRend = GetComponent<SpriteRenderer>();
    }
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

    private void Update() {
        if (GameManager.SabotageId == _sabotageID && Sabotage.SabotageActive) {
            ToggleSabotage(true);
        } else {
            ToggleSabotage(false);
        }
    }

    private void ToggleSabotage(bool state) {
        _box2D.enabled = state;
        _spriteRend.enabled = state;
    }
}
