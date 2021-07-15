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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
