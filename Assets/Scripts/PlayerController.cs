using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float _moveSpeed;      // Player's move speed
    [SerializeField] private Tilemap _obstacleTilemap;
    [SerializeField] private Rigidbody2D _rb;

    private PlayerInput _playerInputs;  // Custom Input Action map for player inputs
    private InputAction _movement;      // Reference to player's movement inputs
    private Vector2 _moveDirection;

    private void Awake() {
        _playerInputs = new PlayerInput(); // Instantiate reference to custom InputAction
        if (HallwaySaveData.IsInitialized) {
            transform.position = HallwaySaveData.NewPosition;
        } else {
            HallwaySaveData.IsInitialized = true;
        }
        
    }

    private void OnEnable() {
        _movement = _playerInputs.Player.Movement;
        _movement.Enable();
    }

    private void OnDisable() {
        _movement.Disable();
    }

    void FixedUpdate() {
        _moveDirection = _movement.ReadValue<Vector2>();
        _rb.velocity = _moveDirection * _moveSpeed * Time.fixedDeltaTime;
    }
}
