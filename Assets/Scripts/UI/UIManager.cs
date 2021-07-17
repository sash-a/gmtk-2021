using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private Character currentHost = null;
    public InfectionBar mainInfectionBar;
    public GameObject visibilityIconPrefab;

    public TextMeshProUGUI remainingHumansText;
    public TextMeshProUGUI remainingZombiesText;
    public TextMeshProUGUI timeText;

    public GameObject characterInfectionBars;
    private void Awake()
    {
        instance = this;
    }

    public static void setCurrentHost(Character host)
    {
        instance.currentHost = host;
        instance.mainInfectionBar.infectedCharacter = host;
    }

    private void Update()
    {
        timeText.text = "Time: " + (int)(Time.time - CharacterManager.instance.levelStartTime);
        remainingHumansText.text = "Humans: " + CharacterManager.instance.getNumberUninfectedHumans();
        remainingZombiesText.text = "Zombies: " + CharacterManager.instance.getNumberInfected();
    }
}
