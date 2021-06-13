using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject winScreen;
    // Start is called before the first frame update
    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (CharacterManager.instance.getNumberOfHumans() == 0)
        {
            winScreen.SetActive(true);
        }
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
