using UnityEngine;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager Instance { get { return _instance;  } }

    private static DialogueManager _instance;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
}
