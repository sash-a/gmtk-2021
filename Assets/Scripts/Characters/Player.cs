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
        
        dir = dir.normalized * (character.moveSpeed * Time.deltaTime);
        moveDirection(dir);
    }
}
