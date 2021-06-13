using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    private int totalStartingHumans;
    private int currentHumans;
    private List<string> musicLayers = new List<string>();
    private float ratio;
    private MusicManager musicManager;

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

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        musicManager = transform.GetComponent<MusicManager>();
        foreach (Sound m in musicManager.music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }


    }

    private void Start()
    {
        foreach(Sound s in musicManager.music)
        {
            musicLayers.Add(s.name);
            playMusicLayer(s.name);
        }

        StartCoroutine(setTotalHumanHack());
        
    }

    private void Update()
    {
       
        if(CharacterManager.instance.getNumberOfHumans() < currentHumans)
        {
            currentHumans = CharacterManager.instance.getNumberOfHumans();
            float numMusicLayers = (totalStartingHumans + 1 - currentHumans) * ratio;
            int numLayers = Mathf.Max(Mathf.RoundToInt(numMusicLayers), 1);
            numLayers = Mathf.Min(numLayers, musicLayers.Count);
            for (int i = 0; i < numLayers; i++)
            {
                setMusicVolume(musicLayers[(int)i], 1);
            }     
        }
    }


    void setMusicVolume(string name, float volume)
    {
        Sound s = Array.Find(musicManager.music, sound => sound.name == name);
        s.source.volume = volume;
    }

    public void playMusicLayer(string name)
    {
        Sound s = Array.Find(musicManager.music, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound: " + name + " not found");
            return;
        }

        s.source.Play();

    }

    IEnumerator setTotalHumanHack()
    {
        yield return new WaitForSeconds(1);
        totalStartingHumans = CharacterManager.instance.getNumberOfHumans();
        currentHumans = totalStartingHumans;
        ratio = musicLayers.Count / totalStartingHumans;

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
