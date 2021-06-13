using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attacker : Character
{
    public float attackRange; // distance guard will stop and shoot at
    public abstract void Attack(Vector3 dir = new Vector3(), bool isPlayer = false);

}
