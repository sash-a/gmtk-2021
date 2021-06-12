using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Character
{
    public Transform shootPoint;

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
        RaycastHit2D hitInfo = Physics2D.Raycast(shootPoint.position, shootPoint.right);
    
        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
        }

    }

    public override void die() {

    }
}
