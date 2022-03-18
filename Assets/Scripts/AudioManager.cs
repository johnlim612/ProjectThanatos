using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour {
    public Sound[] Sounds;
    public static AudioManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        foreach(Sound s in Sounds) {
            if (s.Source == null) {
                s.Source = gameObject.AddComponent<AudioSource>();
            }
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
        }
    }

    public void AssignAudioSource(GameObject sourceGO, string name) {
        Sound s = Array.Find(Sounds, sound => sound.Name == name);
        if (s == null) {
            Debug.LogError("Couldn't Find Sound");
            return;
        }
        s.Source = sourceGO.AddComponent<AudioSource>();
        s.Source.clip = s.Clip;
        s.Source.volume = s.Volume;
        s.Source.pitch = s.Pitch;
        s.Source.loop = s.Loop;
    }

    public void Play(string name) {
        Sound s = Array.Find(Sounds, sound => sound.Name == name);
        if (s == null) {
            Debug.LogError("Couldn't Find Sound");
            return;
        }
        s.Source.Play();
    }

    public void Stop(string name) {
        Sound s = Array.Find(Sounds, sound => sound.Name == name);
        if (s == null) {
            Debug.LogError("Couldn't Find Sound");
            return;
        }
        s.Source.Stop();
    }
}