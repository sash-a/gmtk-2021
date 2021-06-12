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


    // ReSharper disable Unity.PerformanceAnalysis
    void Update()
    { 
        move();
        // findHosts();
        tryAttack();
    }

    // ReSharper disable Unity.PerformanceAnalysis
        private void findHosts()
        {
            HashSet<Controller> visibleHumans = CharacterManager.getVisibleHumans(this);
            
        }
    
    private void jumpHost()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
        { // space not pushed. no jumping/infecting
            return;
        }
        Sauce sauce = GetComponent<Sauce>();
        if (sauce != null)
        { // the player is not in a character. is in sauce form
            tryInfect();
        }
    }
    
    private void tryInfect()
    {//player is in sauce form. we should check if there are any characters in front of us
        foreach (var human in CharacterManager.instance.humans)
        {
            
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
    
    private void tryAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Click");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Attack(mousePos-transform.position);
            
        }
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
