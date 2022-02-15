using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour {
    private string _interactableText = "Press Space";
    private bool _interactableCollided = false;
    private BoxCollider2D _boxCol2D;

    public void OnInteract(InputValue value) {
        if (_interactableCollided && !UI.UIDialogueManager.IsInteracting) {
            InteractObject();
        }
    }

    public virtual void InteractObject() {
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            _interactableCollided = true;
            //print("Collided");
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            _interactableCollided = false;
            //print("left");
        }
    }
    private void OnGUI() {
        if (_interactableCollided) {
            _boxCol2D = GetComponent<BoxCollider2D>();
            var position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            var textSize = GUI.skin.box.CalcSize(new GUIContent(_interactableText));
            GUI.Box(new Rect(position.x - textSize.x/2, Screen.height - position.y + _boxCol2D.size.y/2, textSize.x, textSize.y), _interactableText);
        }
    }
}
