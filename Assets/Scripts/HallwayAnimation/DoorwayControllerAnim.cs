using UnityEngine;
using UnityEngine.InputSystem;

public class DoorwayControllerAnim : MonoBehaviour {
    private PlayerInput _playerInputs;
    private InputAction _movement;
    private Animator _animator;

    private void Awake() {
        _playerInputs = new PlayerInput();
        _animator = GetComponent<Animator>();
        _animator.Play("Doorway");
    }

    private void OnEnable() {
        _movement = _playerInputs.Player.Movement;
        _movement.Enable();
    }

    private void OnDisable() {
        _movement.Disable();
    }

    void Update() {
        float moveVal = _movement.ReadValue<Vector2>().y;
        if (moveVal > 0) {
            _animator.enabled = true;
        } else if (moveVal == 0) {
            _animator.enabled = false;
        }
    }

    // TODO: door opening animation?
    private void BeginDoorSequence() {}
}
