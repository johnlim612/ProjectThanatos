using UnityEngine;

public class DoorAnimation : MonoBehaviour {
    [SerializeField] private Animator _animator;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == Constants.PlayerKey) {
            _animator.Play("DoorOpen");
            Debug.Log("Open");
            _animator.SetBool("door", true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == Constants.PlayerKey) {
            Debug.Log("Close");
            _animator.SetBool("door", false);
        }
    }
}
