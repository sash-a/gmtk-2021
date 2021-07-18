using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonHolder : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public RectTransform startOfList;
    public RectTransform buttonsPanel;
    private void Start()
    {
        string[] levels = new string[12];
        for (int i = 1; i < levels.Length + 1; i++)
        {
            levels[i-1] = "Level " + i;
        }

        spawnLevels(levels);
    }

    public void spawnLevels(string[] levelNames)
    { // create a new button for each level and add them to the UI list
        float totalHeight = Mathf.Max(buttonsPanel.rect.height, startOfList.rect.height * (1+levelNames.Length));
        buttonsPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight); 

        for (int i = 0; i < levelNames.Length; i++)
        {
            string name = levelNames[i];
            GameObject button = Instantiate(levelButtonPrefab, parent:transform);
            LevelButton lvButton = button.GetComponent<LevelButton>();
            lvButton.name = name;
            lvButton.setName(name);
            lvButton.rectTransform.localPosition = startOfList.localPosition - new Vector3(0, i * lvButton.rectTransform.rect.height, 0);
        }

    }
}
