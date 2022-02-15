using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HallwayManager : MonoBehaviour {
    [SerializeField] private Direction _direction;
    [SerializeField] private int _distance = 6;

    private GameObject _player;

    void Start() {
        _player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Vector3 nextPos;

        switch (_direction) {
            case Direction.Up:
                nextPos = Vector3.up;
                break;
            case Direction.Right:
                nextPos = Vector3.right;
                break;
            case Direction.Down:
                nextPos = Vector3.down;
                break;
            case Direction.Left:
                nextPos = Vector3.left;
                break;
            default:
                throw new ArgumentException("Uhhh...This 2D game only has 4 directions " +
                    "(Up, Right, Down, Left) and somehow you entered a 5th one: \"" + 
                    _direction + "\".\nPlease stop trying to break our reality.");
        }

        HallwaySaveData.CurrentPosition = _player.transform.position - nextPos;
        HallwaySaveData.NewPosition = _player.transform.position + (nextPos * _distance * 2);

        //_player.transform.position += nextPos * _distance * 2;
        SceneManager.LoadScene("Hallway");
    }

    private void OnValidate() {
        if (_distance <= 0) {
            _distance = 1;
        }
    }

    enum Direction {
        Up, 
        Right,
        Down,
        Left,
    }
}
