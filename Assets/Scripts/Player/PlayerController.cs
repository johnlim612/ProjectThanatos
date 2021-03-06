using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public PlayerSFX Sfx;
    public Rigidbody RB;
    public Transform PlayerCam;

    [SerializeField] private float _walkSpeed;      // Player's move speed
    [SerializeField] private float _sprintSpeed;    //
    [SerializeField] private float _sensitivity;    // Player's camera sensitivity
    [SerializeField] private GameObject _tabletGameObject;    

    private PlayerInput _playerInputs;  // Custom Input Action map for player inputs
    private InputAction _movement;      // Reference to player's movement inputs
    private InputAction _sprint;
    private InputAction _interact;
    private InputAction _tablet;
    private Vector2 _mouseMovement;
    private float _xRot, _yRot;
    private bool controlEnabled;

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        controlEnabled = true;
        _xRot = 0;
        _yRot = 0;

        _playerInputs = new PlayerInput(); // Instantiate reference to custom InputAction
        _movement = _playerInputs.Player.Movement;
        _sprint = _playerInputs.Player.Sprint;
        _interact = _playerInputs.Player.Interact;
        _tablet = _playerInputs.Player.Tablet;
        _tablet.performed += OpenTablet;
        _tabletGameObject.SetActive(false);

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
        _tablet.Enable();
    }

    private void OnDisable() {
        _movement.Disable();
        _sprint.Disable();
        _interact.Disable();
        _tablet.Disable();
        RB.velocity = Vector3.zero;
    }

    private void PlayerMovement() {
        Vector3 _moveDirection = new Vector3(_movement.ReadValue<Vector2>().x, 0, 
            _movement.ReadValue<Vector2>().y); //Obtain movement direction
        //Change vector direction to fit the current transform direction of player
        Vector3 moveVector = PlayerCam.transform.TransformDirection(_moveDirection);

        //Apply vector to velocity
        if (_sprint.ReadValue<float>() == 1) {
            Sfx.Run();
            RB.velocity = moveVector * _sprintSpeed * Time.deltaTime; 
        } else {
            Sfx.Walk();
            RB.velocity = moveVector * _walkSpeed * Time.deltaTime;
        }
        if ((_moveDirection.x == 0 && _moveDirection.z == 0) || RB.velocity == Vector3.zero) {
            Sfx.Stop();
        }
    }

    private void PlayerCamera() {
        if(controlEnabled) {
            _mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            _yRot += _mouseMovement.x * _sensitivity * Time.deltaTime;
            _xRot -= _mouseMovement.y * _sensitivity * Time.deltaTime;
            _xRot = Mathf.Clamp(_xRot, -35, 60);

            PlayerCam.transform.localRotation = Quaternion.Slerp(PlayerCam.rotation, 
                Quaternion.Euler(_xRot, _yRot, 0), 0.5f);
        }
    }

    private void OpenTablet(InputAction.CallbackContext context) {
        if (context.performed) {
            TabletManager.Instance.ToggleTabletState(!TabletManager.Instance.IsOpened());
        }
    }

    void FixedUpdate() {
        PlayerMovement();
        PlayerCamera();
    }

    public void ControlActive(bool active) {
      if (active) {
          controlEnabled = true;
          _movement.Enable();
          _sprint.Enable();
          _interact.Enable();
      } else {
          controlEnabled = false;
          _movement.Disable();
          _sprint.Disable();
          _interact.Disable();
      }
    }
}
