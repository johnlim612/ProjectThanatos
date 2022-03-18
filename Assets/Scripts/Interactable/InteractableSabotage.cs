using UnityEngine;

public class InteractableSabotage : InteractableObject {
    [SerializeField] private string[] _descriptions;
    [SerializeField] private int _sabotageID;
    private BoxCollider _boxCol;
    private SpriteRenderer _spriteRend;

    private void Awake() {
        _boxCol = GetComponent<BoxCollider>();
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
        _boxCol.enabled = state;
        _spriteRend.enabled = state;
    }
}
