using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [NonSerialized] public Sound[] music;
    public static string format = "mp3"; // can also be wav for higher quality
    public string theme;
    
    public Sound[] getThemeMusic(string themeFolder=null)
    {
        if (themeFolder == null)
        {
            themeFolder = theme;
        }
        string[] folderPaths = Directory.GetDirectories(Application.dataPath + "/Resources/Secret_Sauce_OST/" + themeFolder);
        string[] layerFolderPaths = new string[folderPaths.Length];
        Sound[] audio = new Sound [folderPaths.Length];
        
        for (int i = 0; i < folderPaths.Length; i++)
        {
            string[] temp = folderPaths[i].Split('/');
            int layerNum = int.Parse(temp[temp.Length - 1].Split('.')[0]);
            layerFolderPaths[layerNum - 1] = folderPaths[i];
        }

        for (int i = 0; i < layerFolderPaths.Length; i++)
        {
            //Debug.Log("layer folder: " + layerFolderPaths[i]);
            DirectoryInfo d = new DirectoryInfo(layerFolderPaths[i]);
            FileInfo[] files = d.GetFiles("*." + format);
            if (files.Length != 1)
            {
                throw new Exception("need only one file matching: " + layerFolderPaths[i] + "*." + format);
            }

            string relPath = layerFolderPaths[i].Split(new[] {"Resources/"}, StringSplitOptions.None)[1];
            //Debug.Log("resources rel path: "  + relPath);
            string noExtension = files[0].Name.Split('.')[0];
            AudioClip clip =  (AudioClip)Resources.Load(relPath + "/" + noExtension);
            if (clip == null)
            {
                throw new Exception("failed to load audio file: " + relPath + "/" + files[0].Name);
            }
    
            audio[i] = new Sound(files[0].Name, clip);
        }
        
        return audio;
    }

}
