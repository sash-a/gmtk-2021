using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public static Crosshair instance;

    private void Awake()
    {
        instance = this;
    }

    public void activate()
    {
        gameObject.SetActive(true);
        Cursor.visible = false;
    }

    public void deactivate()
    {
        gameObject.SetActive(false);
        Cursor.visible = true;
    }
}
