using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : Character
{
    public override void die()
    {
        Destroy(gameObject);
    }
}