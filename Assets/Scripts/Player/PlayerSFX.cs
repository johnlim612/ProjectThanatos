using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    private AudioManager _audioManager;
    private bool _walking, _running;

    public void Start() {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    public void Walk() {
        if (!_walking) {
            Stop();
            _walking = true;
            _audioManager.Play("walk");
        }
    }

    public void Run() {
        if (!_running) {
            Stop();
            _running = true;
            _audioManager.Play("run");
        }
    }

    public void Stop() {
        _audioManager.Stop("walk");
        _audioManager.Stop("run");
        _walking = false;
        _running = false;
    }
}