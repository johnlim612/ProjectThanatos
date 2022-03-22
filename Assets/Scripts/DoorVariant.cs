using UnityEngine;

public class DoorVariant : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        GameManager.Instance.ToggleRoomName(true, gameObject.name);
    }

    private void OnTriggerExit(Collider other) {
        GameManager.Instance.ToggleRoomName(false, gameObject.name);
    }
}
