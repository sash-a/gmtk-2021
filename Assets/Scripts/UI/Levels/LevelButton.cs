using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [NonSerialized] public string name = "";  // only has a special name if it is a non-campaign level
    public RectTransform rectTransform;
    public TextMeshProUGUI text;

    public void setName(string name)
    {
        this.name = name;
        text.text = name;
    }
    
    public void loadLevel()
    {
        SceneManager.LoadScene(name);
    }

}
