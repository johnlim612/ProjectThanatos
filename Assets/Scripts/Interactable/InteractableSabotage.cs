using UnityEngine;

public class InteractableSabotage : InteractableObject {
    [SerializeField] private string[] _descriptions;
    //private MeshRenderer _meshRend;

    private void Awake() {
        _boxCol = GetComponent<BoxCollider>();
        //_meshRend = GetComponent<MeshRenderer>();
    }

    public override void InteractObject() {
        ToggleActiveState();
        LightAdjuster.SabotageLevel = LightAdjuster.LightType.NORMAL;
        UI.UIDialogueManager.Instance.InitializeDialogue(UI.EntityType.Alert);
    }

    public bool SabotageActive() {
        return _boxCol.enabled;
    }

    //private void Update() {
    //    if (GameManager.Instance.SabotageId == _sabotageID && Sabotage.IsActive) {
    //        ToggleSabotage(true);
    //    } else {
    //        ToggleSabotage(false);
    //    }
    //}

    //public void ToggleSabotage(bool state) {
    //    _boxCol.enabled = state;
    //    //_meshRend.enabled = state;
    //}
}
