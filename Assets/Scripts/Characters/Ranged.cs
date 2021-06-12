using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Attacker
{
    public float attackRange; // distance guard will stop and shoot at
    public float bulletRange; // range of the gun

    public override void Attack()
    {
        int layerMask = LayerMask.GetMask("player", "zombie", "wall");
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, attackRange, layerMask);

        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
            Character character = hitInfo.transform.GetComponent<Character>();

            if (character) {
                character.die();
            }
            else
            {
                Debug.Log("ranged guard hit: " + hitInfo.transform.gameObject);
            }

            // TODO: Add hit effect on impact
            // Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            
            
        }

    }

    public bool checkCleanLineSight()
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
