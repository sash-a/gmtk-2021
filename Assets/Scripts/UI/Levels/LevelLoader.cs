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
        //campaignLevels.Sort();  // sort by level num, not alpha
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

            bool isInCampaign = isSceneInCampaign(sceneName);
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

    public static bool isSceneInCampaign(string sceneName)
    {
        bool isInCampaign = Regex.Match(sceneName, @"(Level)\s\d").Success;
        return isInCampaign;
    }

    public static string getSceneName(int buildID)
    {
        return System.IO.Path.GetFileNameWithoutExtension( UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( buildID ) );
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
