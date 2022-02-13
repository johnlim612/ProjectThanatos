using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour {
    [SerializeField] private float _moveSpeed = 5f;      // Player's move speed
    [SerializeField] private Tilemap _obstacleTilemap;
    [SerializeField] private Rigidbody2D _rb;

    private PlayerInput _playerInputs;  // Custom Input Action map for player inputs
    private InputAction _movement;      // Reference to player's movement inputs

    private Vector2 _position;   // The player's position
    private Vector3Int _obstacleTile;
    private int _hi;
    private int _hello;

    private void Awake() {
        _playerInputs = new PlayerInput(); // Instantiate reference to custom InputAction
        _position = transform.position;
    }

    private void OnEnable() {
        _movement = _playerInputs.Player.Movement;
        _movement.Enable();
    }

    private void OnDisable() {
        _movement.Disable();
    }

    void FixedUpdate() {
        var moveDirection = _movement.ReadValue<Vector2>();
        _rb.velocity = moveDirection * _moveSpeed * Time.fixedDeltaTime;
    }
}
