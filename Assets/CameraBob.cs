using UnityEngine;

public class CameraBob : MonoBehaviour {
    private Animator _animator;

    void Start() {
        _animator = GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetKey(KeyCode.W)) {
            _animator.SetTrigger("Forward");
        } 
    }
}
