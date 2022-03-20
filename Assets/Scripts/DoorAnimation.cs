using UnityEngine;

public class DoorAnimation : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioManager;
    [SerializeField] private AudioClip _open, _close;

    private void Awake() {
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == Constants.PlayerKey) {
            _animator.Play("DoorOpen");
            //Debug.Log("Open");
            _animator.SetBool("door", true);
            _audioManager.clip = _open;
            _audioManager.Play();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == Constants.PlayerKey) {
            //Debug.Log("Close");
            _animator.SetBool("door", false);
            _audioManager.clip = _close;
            _audioManager.Play();
        }
    }
}
