using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Ai
{
    

    private void FixedUpdate()
    {
        print(checkVisisble(Player.instance.gameObject));
    }
}
