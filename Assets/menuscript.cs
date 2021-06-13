using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuscript : MonoBehaviour
public float level = 1

{

   public void PlayGame (level) {
       Scenemanager.LoadScene(level);

       
       }

    public void QuitGame ()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }


}
