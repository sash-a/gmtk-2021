using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Controller
{

    public static Player instance;
    public static float infectionDistance = 3;
    public static float infectionAngle = 45;
    public static float ejectDistance = 2f;
    
    void Awake()
    {
        base.Awake();
        instance = this;
    }


    // ReSharper disable Unity.PerformanceAnalysis
    void Update()
    { 
        move();
        findHosts();
        tryAttack();
        handleRotation();
    }

    void handleRotation()
    {
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void findHosts()  // finds candidate host humans. checks for space input to jump
    {
        HashSet<Controller> visibleHumans = CharacterManager.getVisibleHumans(this);
        if (visibleHumans.Count == 0)
        {// noone in range
            noHostCandidates();
            // Debug.Log("no one in range");
            return;
        }

        Controller targetHuman = null;
        // if multiple humans in range, must select the one which is closest
        float minDistance = float.MaxValue;
        foreach (var human in visibleHumans)
        {
            float distance = Vector3.Distance(transform.position, human.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                targetHuman = human;
            }
        }

        if (targetHuman == null)
        {
            throw new Exception("should never happen");
        }

        targetHuman.glow();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpToHost(targetHuman);
        }
    }

    private void noHostCandidates()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GetComponent<Sauce>() == null) // cannot eject from sauce
            {
                character.eject();
                CharacterManager.humanify(this);
            }
        }    
    }

    private void jumpToHost(Controller targetHuman)
    { // the human should be given a player controller, and the current player object should be destroyed
        CharacterManager.bodySnatch(targetHuman, this);
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
        
        character.SpriteController.legs.transform.rotation = Quaternion.LookRotation(transform.forward, dir);
        character.SpriteController.legsAnimator.SetFloat("Speed", dir.magnitude);

        
        dir = dir.normalized;
        moveDirection(dir);
    }
    
    private void tryAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Attack(mousePos-transform.position);
            
        }
    }

    void Attack(Vector3 mousePos)
    {
        // // Get attack script
        // Attacker attacker = GetComponent<Attacker>();
        //
        // if (attacker)
        // {
        //     RaycastHit2D hitInfo = Physics2D.Raycast(attacker.transform.position, mousePos, attacker.attackRange);
        //
        //     if (hitInfo)
        //     {
        //         Debug.Log(hitInfo.transform.name);
        //         Character character = hitInfo.transform.GetComponent<Character>();
        //
        //         if (character) {
        //             character.die();
        //         }
        //
        //         // Draw ray for testing
        //
        //         // TODO: Add hit effect on impact
        //         // Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
        //     }
        // }
      
    }
}
