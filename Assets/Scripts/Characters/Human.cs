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
        // Debug.DrawLine(transform.position, (transform.position + transform.forward) * 3, Color.red);
        // Debug.DrawLine(transform.position, transform.position + agent.velocity, Color.blue);
        Vector3 targetPosition = transform.position + agent.velocity.normalized;
        Vector3 dir = targetPosition - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}
