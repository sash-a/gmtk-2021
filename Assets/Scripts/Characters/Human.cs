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

    private void LateUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward);
        var newDir = agent.velocity.normalized;
        // newDir.x = transform.rotation.x;
        // newDir.y = transform.rotation.y;
        transform.rotation = Quaternion.LookRotation(transform.forward, newDir);
        // transform.rotation = Quaternion.LookRotation(newDir);
        // transform.rotation=Quaternion.SetLookRotation(agent.velocity);
    }
}
