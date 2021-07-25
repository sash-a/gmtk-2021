using UnityEngine;

public class LevelButtonHolder : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public RectTransform startOfList;
    public RectTransform buttonsPanel;

    public bool isSpecialLevelsHolder;
    private void Start()
    {
        if (isSpecialLevelsHolder)
        {
            spawnLevelButtons(LevelLoader.instance.specialLevels.ToArray());
        }
        else
        {
            spawnLevelButtons(LevelLoader.instance.campaignLevels.ToArray());
        }
    }

    public void spawnLevelButtons(string[] levelNames)
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
