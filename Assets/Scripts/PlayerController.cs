using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float _moveSpeed = 5f;      // Player's move speed=

    private PlayerInput _playerInputs;  // Custom Input Action map for player inputs
    private InputAction _movement;      // Reference to player's movement inputs

    private Vector2 _position;   // The player's position

    private void Awake() {
        _playerInputs = new PlayerInput(); // Instantiate reference to custom InputAction
    }

    private void OnEnable() {
        _movement = _playerInputs.Player.Movement;
        _movement.Enable();
    }

    private void OnDisable() {
        _movement.Disable();
    }

    // Update is called once per frame
    void Update() {
        var moveDirection = _movement.ReadValue<Vector2>();
        _position += _moveSpeed * moveDirection * Time.deltaTime;

        transform.position = _position;
    }
}
