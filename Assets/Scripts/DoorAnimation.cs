using UnityEngine;

public class DoorAnimation : MonoBehaviour {
    [SerializeField] private Animator _animator;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == Constants.PlayerKey) {
            GameManager.Instance.ToggleRoomName(true, gameObject.name);
            _animator.SetBool("door", true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == Constants.PlayerKey) {
            GameManager.Instance.ToggleRoomName(false, gameObject.name);
            _animator.SetBool("door", false);
        }
    }
}
