using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Rigidbody2D))]


public class Controller : MonoBehaviour
{
    
    [NonSerialized] public Character character;
    [NonSerialized] public Rigidbody2D rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
    }

    public void moveDirection(Vector2 dir)
    {
        rb.MovePosition(((Vector2)transform.position) + dir * (character.moveSpeed * Time.deltaTime));
    }

    void Attack()
    {
        // Get attack script
        Attacker attacker = GetComponent<Attacker>();

        // Check if attack component is attached
        if (attacker)
        {
            attacker.Attack();
        }
          
    }

}
