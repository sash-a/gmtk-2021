using System;
using System.IO;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public Sound[] music;
    public float volume = 0.5f;
    public static string format = "mp3"; // can also be wav for higher quality
    public static MusicManager instance;
    public float startFrac; //  ~[0,1] how far into the song progression to start 

    private void Awake()
    {
        instance = this;
        
    }
}
