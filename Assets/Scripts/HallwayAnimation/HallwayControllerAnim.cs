using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Plays/pauses animation based on player movement
/// </summary>
public class HallwayControllerAnim : MonoBehaviour {
    [SerializeField] [Range(0, 1)] private float _startingTime;
    [SerializeField] private DoorType _doorType;

    private PlayerInput _playerInputs;
    private InputAction _movement;
    private Animator _animator;
    private RawImage _image;
    private bool _doneLoading;
    private string _mapSceneName;
    private static int _currInteractions;
    private const int _numInterations = 1;

    private void Awake() {
        _playerInputs = new PlayerInput();
        _animator = GetComponent<Animator>();
        _image = GetComponent<RawImage>();
        _animator.Play("Hallway", -1, _startingTime);
        _doneLoading = false;
        _mapSceneName = HallwaySaveData.MapSceneName;
        _currInteractions = 0;
        StartCoroutine(Load(0.01f));

        if (_doorType != DoorType.Neither) {
            _image.enabled = false;
        }
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
        } else if(moveVal == 0 && _doneLoading){
            _animator.enabled = false;
        } else if (moveVal < 0 && _doneLoading) {
            HallwaySaveData.NewPosition = HallwaySaveData.CurrentPosition;
            SceneManager.LoadScene(_mapSceneName);
        }
    }

    /// <summary>
    /// Gives images a chance to get to their proper timeframe when scene starts.
    /// </summary>
    /// <param name="time">Time given to load</param>
    IEnumerator Load(float time) {
        yield return new WaitForSeconds(time);
        _doneLoading = true;
    }

    /// <summary>
    /// Called after animation ends to re-order image in hierarchy and restart
    /// </summary>
    private void AnimationEnd() {
        transform.SetAsFirstSibling();

        if (_currInteractions < _numInterations) {
            _currInteractions++;
        } else {
            _image.enabled = true;
        }
    }

    /// <summary>
    /// Doors opening animation
    /// </summary>
    private void OpenDoors() {
        if (_doorType != DoorType.Neither && _image.enabled) {
            _doneLoading = false;
            _animator.enabled = true;

            if (_doorType == DoorType.Right) {
                SetDoorBackground();
                _animator.SetTrigger("RightDoor");
            } else {
                _animator.SetTrigger("LeftDoor");
            }
        }
    }

    /// <summary>
    /// Takes the background "DoorBackground" and makes its opacity 1.
    /// This should be used after the "RightDoor" has been triggered becuase it
    /// it above the "LeftDoor" in the hierachy.
    /// </summary>
    private void SetDoorBackground() {
        RawImage image = GameObject.Find("DoorBackground").GetComponent<RawImage>();
        Color color = image.color;
        int idx = transform.GetSiblingIndex();

        image.transform.SetSiblingIndex(idx);
        color.a = 1;
        image.color = color; 
    }

    /// <summary>
    /// Goes to next room/scene
    /// </summary>
    private void NextRoom() {
        SceneManager.LoadScene(_mapSceneName);
    }

    enum DoorType {
        Neither,
        Right,
        Left
    }
}
