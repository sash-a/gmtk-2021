using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    /*
     * Finds all level scenes and special levels
     */

    [NonSerialized] public LevelLoader instance;

    private void Awake()
    {
        instance = this;
    }
}
