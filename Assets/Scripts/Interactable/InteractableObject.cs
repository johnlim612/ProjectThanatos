using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour {
    protected string _interactableText = "Press Space";
    protected bool _interactableCollided = false;
    protected BoxCollider _boxCol;
    public bool ActiveQuest = false;
    public int Id { get; private set; }
    protected bool _isActive = false;

    public void OnInteract(InputValue value) {
        if (_interactableCollided && !UI.UIDialogueManager.Instance.IsInteracting) {
            InteractObject();
        }
    }

    public virtual void InteractObject() {}

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.CompareTag(Constants.PlayerKey)) {
            _interactableCollided = true;
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.gameObject.CompareTag(Constants.PlayerKey)) {
            _interactableCollided = false;
        }
    }

    public void ToggleActiveState() {
        IsActive = !IsActive;
        _boxCol.enabled = IsActive;
    }

    //private void OnGUI() {
    //    if (_interactableCollided) {
    //        _boxCol = GetComponent<BoxCollider>();
    //        var position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    //        var textSize = GUI.skin.box.CalcSize(new GUIContent(_interactableText));
    //        float x = position.x - textSize.x / 2;
    //        float y = Screen.height - position.y + _boxCol.size.y / 2;
    //        GUI.Box(new Rect(x, y, textSize.x, textSize.y), _interactableText);
    //    }
    //}

    public bool IsActive {
        get { return _isActive; }
        protected set { _isActive = value; }
    }
}
