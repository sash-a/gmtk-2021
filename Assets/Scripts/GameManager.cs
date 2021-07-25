using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject winScreen;

    void Update()
    {
        if (CharacterManager.instance.getNumberUninfectedHumans() == 0) { winScreen.SetActive(true); }
        
        if (Input.GetKeyDown(KeyCode.R)) { Restart(); }
    }

    public void SwitchScene()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        string currentSceneName = LevelLoader.getSceneName(currentBuildIndex);
        string name;
        if (LevelLoader.isSceneInCampaign(currentSceneName))
        {
            List<string> scenesInBuild = new List<string>();
            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                int lastSlash = scenePath.LastIndexOf("/");
                scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
            }
            
            
            int nextLevel = int.Parse(currentSceneName.Split(' ')[1]) + 1;
            while (nextLevel < 50)
            {
                name = "Level " + nextLevel;
                Debug.Log("name: " + name + " scene: " + SceneManager.GetSceneByName(name) + " eq: " + (SceneManager.GetSceneByName(name) == SceneManager.GetSceneByName("NOTASCENEXOXOXOXO")));
                if (scenesInBuild.Contains(name))
                {  // valid scene
                    SceneManager.LoadScene(name);
                    break;
                }

                nextLevel++;
            }
        }
        else
        {
            SceneManager.LoadScene("Start");
        }

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
