using UnityEngine;

public class InteractableSabotage : InteractableObject {
    [SerializeField] private string[] _descriptions;
    [SerializeField] private int _sabotageID;
    //private MeshRenderer _meshRend;

    private void Awake() {
        _boxCol = GetComponent<BoxCollider>();
        //_meshRend = GetComponent<MeshRenderer>();
    }

    public int SabotageID {
        get {
            return _sabotageID;
        }
    }

    public override void InteractObject() {
        GameManager.Instance.ClearSabotage();
    }

    private void Update() {
        if (GameManager.Instance.SabotageId == _sabotageID && Sabotage.SabotageActive) {
            ToggleSabotage(true);
        } else {
            ToggleSabotage(false);
        }
    }

    private void ToggleSabotage(bool state) {
        _boxCol.enabled = state;
        //_meshRend.enabled = state;
    }
}
