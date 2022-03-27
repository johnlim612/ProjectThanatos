using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound {
    public string Name;
    [HideInInspector]
    public int UniqueID;
    [HideInInspector]
    public AudioSource Source;
    public AudioClip Clip;
    [Range(0f, 1f)]
    public float Volume;
    [Range(.1f, 3f)]
    public float Pitch;
    public bool Loop;
 
}