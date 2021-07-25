using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager instance;

    public GameObject zombiePoint;
    
    public GameObject redBloodPuddle;
    public GameObject greenBloodPuddle;
    public GameObject dieEffect;
    public GameObject greenDieEffect;
    
    public GameObject saucePrefab;


    private void Awake()
    {
        instance = this;
    }
}
