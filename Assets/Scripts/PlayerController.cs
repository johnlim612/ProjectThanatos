using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour {
    [SerializeField] private float _moveSpeed = 5f;      // Player's move speed
    [SerializeField] private Tilemap _obstacleTilemap;

    private PlayerInput _playerInputs;  // Custom Input Action map for player inputs
    private InputAction _movement;      // Reference to player's movement inputs

    private Vector2 _position;   // The player's position
    private Vector3Int _obstacleTile;
    private int _hi;
    private int _hello;

    private void Awake() {
        _playerInputs = new PlayerInput(); // Instantiate reference to custom InputAction
        transform.position = new Vector3(8, 9, 0);
    }

    private void OnEnable() {
        _movement = _playerInputs.Player.Movement;
        _movement.Enable();
    }

    private void OnDisable() {
        _movement.Disable();
    }

    void Update() {
        var moveDirection = _movement.ReadValue<Vector2>();
        var posIncrement = _moveSpeed * moveDirection * Time.deltaTime;

        if (moveDirection.y > 0) {
            _hi = 1;
        } else {
            _hi = 0;
        }

        _obstacleTile = _obstacleTilemap.WorldToCell(_position + posIncrement + new Vector2Int(0, _hi));
        _position += _moveSpeed * moveDirection * Time.deltaTime;
        transform.position = _position;
        /*        if (_obstacleTilemap.GetTile(_obstacleTile) == null) {
                    _position += posIncrement;
                    transform.position = _position;
                }*/
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_position + new Vector2Int(_hello, _hi), 0.2f);
    }
}
