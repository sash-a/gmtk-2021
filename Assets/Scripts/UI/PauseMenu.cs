using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private Canvas pauseMenu;

    void Start() {
        pauseMenu.enabled = false;
        isPaused = false;
    }

    void Update() {
        if (Input.GetKey("escape")) {
            Pause();
        }
    }

    public void Pause() {
        isPaused = true;
        pauseMenu.enabled = true;
    }

    public void Resume() {
        isPaused = false;
        pauseMenu.enabled = false;
    }

    public void QuitGame() {
        Application.Quit();
    }
}
