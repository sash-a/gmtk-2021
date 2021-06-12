using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Character
{
    public Transform shootPoint;
    public float shootRange;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Shoot");
            shoot();
        }

        Debug.DrawRay(shootPoint.position, shootPoint.right, Color.red);
    }

    void shoot()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(shootPoint.position, shootPoint.right, range);
    
        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
            Character character = hitInfo.transform.GetComponent<Character>();
            
            if (character) {
                character.die();
            }

            // Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
        }

    }

    public override void die() {
        Destroy(gameObject);
    }
}
