using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float _moveSpeed;      // Player's move speed
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _sensitivity;    // Player's camera sensitivity
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _camera;

    private PlayerInput _playerInputs;  // Custom Input Action map for player inputs
    private InputAction _movement;      // Reference to player's movement inputs
    private InputAction _sprint;
    private InputAction _interact;
    private Vector2 _mouseMovement;
    private float _xRot, _yRot;

    private void Awake() {
        _xRot = 0;
        _yRot = 0;
        Cursor.lockState = CursorLockMode.Locked;
        _playerInputs = new PlayerInput(); // Instantiate reference to custom InputAction
        _movement = _playerInputs.Player.Movement;
        _sprint = _playerInputs.Player.Sprint;
        _interact = _playerInputs.Player.Interact;
        
        /*
        if (HallwaySaveData.IsInitialized) {
            transform.position = HallwaySaveData.NewPosition;
        } else {
            HallwaySaveData.IsInitialized = true;
        }
        */
    }

    private void OnEnable() {
        _movement.Enable();
        _sprint.Enable();
        _interact.Enable();
    }

    private void OnDisable() {
        _movement.Disable();
        _sprint.Disable();
        _interact.Disable();
    }
    
    private void PlayerMovement() {
        Vector3 _moveDirection = new Vector3(_movement.ReadValue<Vector2>().x, 0, _movement.ReadValue<Vector2>().y); //Obtain movement direction
        Vector3 moveVector = _camera.transform.TransformDirection(_moveDirection); //Change vector direction to fit the current transform direction of player
        if (_sprint.ReadValue<float>() == 1) {
            _rb.velocity = moveVector * _sprintSpeed * Time.deltaTime; //Apply vector to velocity
        } else {
            _rb.velocity = moveVector * _moveSpeed * Time.deltaTime;
        }
    }

    private void PlayerCamera() {
        _mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        _yRot += _mouseMovement.x * _sensitivity * Time.deltaTime;
        _xRot -= _mouseMovement.y * _sensitivity * Time.deltaTime;
        _xRot = Mathf.Clamp(_xRot, -35, 60);

        _camera.transform.localRotation = Quaternion.Slerp(_camera.rotation, Quaternion.Euler(_xRot, _yRot, 0), 0.5f);
        
    }

    private void PlayerInteract() {
        if (_interact.ReadValue<float>() == 1) {
            Debug.Log("Interacted");
        }
    }

    void FixedUpdate() {
        PlayerMovement();
        PlayerCamera();
        PlayerInteract();
    }
}
