using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attacker : Character
{
    public float attackRange; // distance guard will stop and shoot at
    [NonSerialized] public Animator playerState;
    
    public new void Awake()
    {
        base.Awake();
        playerState = GetComponent<Animator>();
    }
}
