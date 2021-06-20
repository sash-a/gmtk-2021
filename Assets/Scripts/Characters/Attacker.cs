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
    
    public abstract void Attack(Vector3 dir = new Vector3(), bool isPlayer = false);

    public bool CheckCleanLineSight()
    {    
        int layerMask = LayerMask.GetMask("player", "zombie", "wall");
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, attackRange, layerMask);
        if (hitInfo)
        {
            Character character = hitInfo.transform.GetComponent<Character>();

            if (character)
            {
                return true;
            }
        }

        return false;
    }
}
