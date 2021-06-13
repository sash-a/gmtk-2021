using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human : Ai
{
    private void Start()
    {
        CharacterManager.registerHuman(this);
        base.Start();   
    }

    private void FixedUpdate()
    {

        // print($"dest:{agent.destination}|player:{Player.instance.transform.position}");
            // print("visible:" + checkVisisble(Player.instance.gameObject));
        // agent.SetDestination(Player.instance.transform.position);

        if (Input.GetKeyUp(KeyCode.Z))
        {
            character.infect();
        }

        base.FixedUpdate();

    }

    // private void LateUpdate()
    // {
    //     // Debug.DrawLine(transform.position, (transform.position + transform.forward) * 3, Color.red);
    //     // Debug.DrawLine(transform.position, transform.position + agent.velocity, Color.blue);
    //
    // }
}
