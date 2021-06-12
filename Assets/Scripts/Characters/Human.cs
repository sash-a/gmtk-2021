using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Ai
{

    private void FixedUpdate()
    {
        print("visible:" + checkVisisble(Player.instance.gameObject));
    }
}
