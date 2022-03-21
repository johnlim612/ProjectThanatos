using UnityEngine;

public class DoorAnimation : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioManager;
    [SerializeField] private AudioClip _open, _close;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Constants.PlayerKey)) {
            GameManager.Instance.ToggleRoomName(true, gameObject.name);
            _animator.SetBool("door", true);
            _audioManager.clip = _open;
            _audioManager.Play();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(Constants.PlayerKey)) {
            GameManager.Instance.ToggleRoomName(false, gameObject.name);
            _animator.SetBool("door", false);
            _audioManager.clip = _close;
            _audioManager.Play();
        }
    }
}
