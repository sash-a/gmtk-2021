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
    }

    private void Start()
    {
        foreach (Sound m in MusicManager.instance.music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }
        
        foreach(Sound s in MusicManager.instance.music)
        {
            musicLayers.Add(s.name);
            playMusicLayer(s.name);
        }

        StartCoroutine(setTotalHumanHack());
        
    }

    private void Update()
    {
       
        if(CharacterManager.instance.getNumberUninfectedHumans() < currentHumans)
        {
            //Debug.Log("humans lost one. starting: " + totalStartingHumans + " current: " + currentHumans);
            currentHumans = CharacterManager.instance.getNumberUninfectedHumans();
            float numMusicLayers = (totalStartingHumans + 1 - currentHumans) * ratio;
            //Debug.Log("num music layers: " + numMusicLayers + "ratio: " + ratio);
            int numLayers = Mathf.Max(Mathf.RoundToInt(numMusicLayers), 1);
            numLayers = Mathf.Min(numLayers, musicLayers.Count);
            for (int i = 0; i < numLayers; i++)
            {
                setMusicVolume(musicLayers[(int)i], MusicManager.instance.volume);
            }     
        }
    }


    void setMusicVolume(string name, float volume)
    {
        Sound s = Array.Find(MusicManager.instance.music, sound => sound.name == name);
        s.source.volume = volume;
    }

    public void playMusicLayer(string name)
    {
        Sound s = Array.Find(MusicManager.instance.music, sound => sound.name == name);
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
        //Debug.Log("num humans: " + totalStartingHumans);
        currentHumans = totalStartingHumans;
        ratio = (float)musicLayers.Count / (float)totalStartingHumans;
        //Debug.Log("num music layers: " + musicLayers.Count + " ratio: " + ratio);
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
