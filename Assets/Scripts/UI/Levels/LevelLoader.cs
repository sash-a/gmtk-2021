using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    /*
     * Finds all level scenes and special levels
     */

    [NonSerialized] public static LevelLoader instance;
    [NonSerialized] public List<string> campaignLevels;
    [NonSerialized] public List<string> specialLevels;
    private static string[] excludedScenes = new[] { "Win Scene", "Start"};


    private void Awake()
    {
        instance = this;
        specialLevels = getAllLevels(getSpecialLevels: true);
        campaignLevels = getAllLevels(getSpecialLevels: false);
        campaignLevels.Sort();
    }

    public List<string> getAllLevels(bool getSpecialLevels=false)
    {
        string[] scenes = getAllBuildSceneNames();
        List<string> levels = new List<string>();
        foreach (var sceneName in scenes)
        {
            if (excludedScenes.Contains(sceneName))
            {
                continue;
            }

            bool isInCampaign = Regex.Match(sceneName, @"(Level)\s\d").Success;
            if (isInCampaign && !getSpecialLevels)
            {
                levels.Add(sceneName);
            }
            else if (!isInCampaign && getSpecialLevels) {
                levels.Add(sceneName);
            }
        }
        return levels;
    }
    
    public string[] getAllBuildSceneNames(){
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
        string[] scenes = new string[sceneCount];
        for( int i = 0; i < sceneCount; i++ )
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension( UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( i ) );
        }

        return scenes;
    }
}
