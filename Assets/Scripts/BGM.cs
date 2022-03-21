using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {
    private AudioManager _audioManager;
    
    // Start is called before the first frame update
    void Start() {
        _audioManager = FindObjectOfType<AudioManager>();
        _audioManager.Play("bgm");
    }
}
