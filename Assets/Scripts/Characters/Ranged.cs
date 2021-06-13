using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Attacker
{
    public float attackRange; // distance guard will stop and shoot at
    public float bulletRange; // range of the gun
    public float fireRate;
    private float lastFire;

    public override void Attack(Vector3 dir = new Vector3(), bool isPlayer = false)
    {
        if (Time.time - lastFire < fireRate)
        {
            return;
        }
        lastFire = Time.time;

        StartCoroutine(WaitForFire(dir, isPlayer));
    }
    
    public void DoAttack(Vector3 dir = new Vector3(), bool isPlayer = false)
    {
        if (dir == Vector3.zero)
            dir = transform.right;

        dir = dir.normalized;

        int layerMask = LayerMask.GetMask("player", "zombie", "wall");
        if(isPlayer)
            layerMask = LayerMask.GetMask( "wall", "human");

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, dir, attackRange, layerMask);

        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
            Character character = hitInfo.transform.GetComponent<Character>();

            if (character) {
                character.die();
            }
            else {
                Debug.Log("ranged guard hit: " + hitInfo.transform.gameObject);
            }

            // TODO: Add hit effect on impact
            // Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            Debug.Log("SHOOTING!");
        }

    }

    public IEnumerator WaitForFire(Vector3 dir = new Vector3(), bool isPlayer = false)
    {
        Debug.Log("Waiting for fire");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("firing!");
        DoAttack(dir, isPlayer);
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
