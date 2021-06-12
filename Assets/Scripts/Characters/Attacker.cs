using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attacker : Character
{
    public abstract void Attack();

    public override void die()
    {
        Destroy(gameObject);
    }

}
