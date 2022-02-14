using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Diary : MonoBehaviour
{
    private bool _interactableCollided = false;
    private string _interactableText = "Press Space";
    // Start is called before the first frame update
    void Start() {
    }

    public void OnInteract(InputValue value) {
        if (_interactableCollided) {
            InteractObject();
        }
    }
    private void InteractObject() {
        //FindObjectOfType<UIDialogueManager>().StartDialog(this.gameObject);
        //GameManager.advanceDay();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            _interactableCollided = true;
            print("Collided");
        }
        
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            _interactableCollided = false;
            print("left");
        }
    }
    private void OnGUI() {
        if (_interactableCollided) {
            var position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            var textSize = GUI.skin.box.CalcSize(new GUIContent(_interactableText));
            GUI.Box(new Rect(position.x - textSize.x/2, Screen.height - position.y + textSize.y/2, textSize.x, textSize.y), _interactableText);
        }
    }
}
