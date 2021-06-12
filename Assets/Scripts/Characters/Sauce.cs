using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sauce : Character
{
    private void Awake()
    {
        UIManager.setCurrentHost(this);
    }

    public override void die()
    {
        throw new Exception("player has died");
    }
    
}
