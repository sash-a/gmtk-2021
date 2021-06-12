using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Character
{
    public Transform attackPoint;
    public float attackRange;
   
    public void Attack()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(attackPoint.position, attackPoint.right, attackRange);

        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
            Character character = hitInfo.transform.GetComponent<Character>();

            if (character) {
                character.die();
            }

            // TODO: Add hit effect on impact
            // Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
        }

    }

    public override void die()
    {
        Destroy(gameObject);
    }

}
