using UnityEngine;
using UnityEngine.InputSystem;

public class CameraBobAnim : MonoBehaviour {
    private Animator _animator;
    private PlayerInput _playerInputs;
    private InputAction _movement;

    private void Awake() {
        _playerInputs = new PlayerInput();
    }

    void Start() {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        _movement = _playerInputs.Player.Movement;
        _movement.Enable();
    }

    private void OnDisable() {
        _movement.Disable();
    }

    void Update() {
        if (_movement.ReadValue<Vector2>().y > 0) {
            _animator.SetTrigger("Forward");
        }
    }
}
