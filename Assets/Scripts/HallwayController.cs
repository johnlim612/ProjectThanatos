using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Moves the image closer to the player with a fade in/out effect.
/// </summary>
public class HallwayController : MonoBehaviour {
    [SerializeField] private float _increment;
    [SerializeField] private float _fadeInVal;
    [SerializeField] private float _fadeOutVal;
    [SerializeField] private RawImage _image;

    private Vector3 _nextPos;
    private Vector3 _nextScale;
    private float _posIncrX;
    private float _posIncrY;
    private float _alphaIncr;
    private float _alphaDecr;
    private Color _color;
    private PlayerInput _playerInputs;
    private InputAction _movement;  

    private readonly Vector3 _startPos = new Vector3(15.5f, 103, 0);
    private readonly Vector3 _startScale = new Vector3(7, 7, 1);
    private readonly Vector3 _endPos = new Vector3(202, 1235, 0);
    private readonly Vector3 _endScale = new Vector3(93, 93, 1);

    private void Awake() {
         _playerInputs = new PlayerInput();
    }

    void Start() {
        // calculate incremental values for position and alpha based on scale increment
        float numIncrements = (_endScale.x - _startScale.x) / _increment;
        _posIncrX = (_endPos.x - _startPos.x) / numIncrements;
        _posIncrY = (_endPos.y - _startPos.y) / numIncrements;
        _alphaIncr = 1 / ((_fadeInVal - _startScale.x) / _increment);
        _alphaDecr = 1 / ((_endScale.x - _fadeOutVal) / _increment);

        _nextScale = transform.localScale;
        _nextPos = transform.localPosition;
        
        _color = _image.color;
        _color.a = 0;
        _image.color = _color;
    }

    private void OnEnable() {
        _movement = _playerInputs.Player.Movement;
        _movement.Enable();
    }

    private void OnDisable() {
        _movement.Disable();
    }

    void Update() {
        float yMovement = _movement.ReadValue<Vector2>().y;
        if (yMovement > 0) {
            IncrementImage();
            IncrementImageFade();

            if (_nextScale.x >= _endScale.x) {
                SetToStart();
            }
        } else if (yMovement < 0) {
            print("Player wants to go back!");
        }
    }

    /// <summary>
    /// Fade in/out image based on its size when approaching the player
    /// </summary>
    private void IncrementImageFade() {
            if (_nextScale.x <= _fadeInVal) {
                _color.a += _alphaIncr;
                _image.color = _color;
            } else if (_nextScale.x >= _fadeOutVal) {
                _color.a -= _alphaDecr;
                _image.color = _color;
            }
    }

    /// <summary>
    /// Fade in/out image based on its size when moving away from the player.
    /// This is supposed to revervse the effects of IncrementImageFade().
    /// </summary>
    private void DecrementImageFade() {
        if (_nextScale.x <= _fadeInVal) {
            _color.a -= _alphaIncr;
            _image.color = _color;
        } else if (_nextScale.x >= _fadeOutVal) {
            _color.a += _alphaDecr;
            _image.color = _color;
        }
    }

    /// <summary>
    /// Moves the image "closer" to the player by making it larger
    /// </summary>
    private void IncrementImage() {
        _nextScale.x += _increment;
        _nextScale.y += _increment;
        _nextPos.x += _posIncrX;
        _nextPos.y += _posIncrY;

        transform.localScale = _nextScale;
        transform.localPosition = _nextPos;
    }

    /// <summary>
    /// Moves the image "away" from the player by making is smaller
    /// </summary>
    private void DecrementImage() {
        _nextScale.x -= _increment;
        _nextScale.y -= _increment;
        _nextPos.x -= _posIncrX;
        _nextPos.y -= _posIncrY;

        transform.localScale = _nextScale;
        transform.localPosition = _nextPos;
    }

    /// <summary>
    /// Sets the image to the start (when image is furthest from player)
    /// </summary>
    private void SetToStart() {
        transform.localScale = _startScale;
        transform.localPosition = _startPos;
        _nextScale = _startScale;
        _nextPos = _startPos;
        _color.a = 0;
        _image.color = _color;
        transform.SetAsFirstSibling();
    }

    /// <summary>
    /// Sets the image to the end (when image is closest to player)
    /// </summary>
    private void SetToEnd() {
        transform.localScale = _endScale;
        transform.localPosition = _endPos;
        _nextScale = _endScale;
        _nextPos = _endPos;
        _color.a = 0;
        _image.color = _color;
        transform.SetAsLastSibling();
    }
}
