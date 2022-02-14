using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Plays/pauses animation
/// </summary>
public class HallwayController : MonoBehaviour {
    [SerializeField] [Range(0, 1)] private float _startingTime;

    private PlayerInput _playerInputs;
    private InputAction _movement;
    private Animator _animator;
    private bool _doneLoading;

    private void Awake() {
        _playerInputs = new PlayerInput();
        _animator = GetComponent<Animator>();
        _animator.Play("Hallway", -1, _startingTime);
        _doneLoading = false;
        StartCoroutine(Load(0.01f));
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
        } else {
            // TODO: Player goes back to previous room
            print("Player wants to go back!");
        }
    }

    /// <summary>
    /// Gives images a chance to get to their proper timeframe when scene starts.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator Load(float time) {
        yield return new WaitForSeconds(time);
        _doneLoading = true;
    }

    /// <summary>
    /// Called after animation ends to re-order image in hierarchy and restart
    /// </summary>
    private void AnimationEnd() {
        transform.SetAsFirstSibling();
    }
}
