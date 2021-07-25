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
            int nextLevel = int.Parse(currentSceneName.Split(' ')[1]) + 1;
            name = "Level " + nextLevel;
            SceneManager.LoadScene(name);
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
