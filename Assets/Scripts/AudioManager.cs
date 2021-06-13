using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        // Play("music");
    }

   public void Play(string name)
   {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogError("Sound: " + name + " not found");
            return;
        }

        s.source.Play();

   }

    public void PlayRandom(string[] names)
    {
        List<Sound> soundList = new List<Sound>();
        foreach(string name in names)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            soundList.Add(s);
        }

        int rand = new System.Random().Next(0, soundList.Count);
        soundList[rand].source.Play();
    }
}
