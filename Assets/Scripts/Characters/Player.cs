using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Controller
{

    public static Player instance;

    void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            throw new Exception("WTF are you doing there is only one player!!!!!!!!!!!!!!!!!!");
        }
        instance = this;
    }


    // Update is called once per frame
    void Update()
    { 
        move();

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Click");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Attack(mousePos-transform.position);
            
        }
    }

    void move()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir += Vector2.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            dir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir += Vector2.down;
        }
        
        dir = dir.normalized;
        moveDirection(dir);
    }

    void Attack(Vector3 mousePos)
    {
        // Get attack script
        Attacker attacker = GetComponent<Attacker>();

        if (attacker)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(attacker.attackPoint.position, mousePos, attacker.attackRange);

            if (hitInfo)
            {
                Debug.Log(hitInfo.transform.name);
                Character character = hitInfo.transform.GetComponent<Character>();

                if (character) {
                    character.die();
                }

                // Draw ray for testing

                // TODO: Add hit effect on impact
                // Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            }
        }
      
    }
}
