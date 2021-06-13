using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sauce : Character
{
    public Animator sauceAnimator;
    private void Awake()
    {
        UIManager.setCurrentHost(this);
    }
}
