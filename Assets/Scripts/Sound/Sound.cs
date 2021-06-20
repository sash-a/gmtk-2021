using UnityEngine.Audio;
using UnityEngine;
using System;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public bool loop;

    [HideInInspector]
    public AudioSource source;

    public Sound(string name, AudioClip clip)
    {
        this.name = name;
        this.clip = clip;
        volume = 0;
        pitch = 1;
        loop = true;
    }
}
