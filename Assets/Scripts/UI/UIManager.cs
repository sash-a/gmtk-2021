using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private Character currentHost = null;
    public InfectionBar mainInfectionBar;

    private void Awake()
    {
        instance = this;
    }

    public static void setCurrentHost(Character host)
    {
        instance.currentHost = host;
        instance.mainInfectionBar.infectedCharacter = host;
    }
}
