using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human : Ai
{
    private NavMeshAgent agent;
    private void Start()
    {
        CharacterManager.registerHuman(this);
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        print($"dest:{agent.destination}|player:{Player.instance.transform.position}");
        // print("visible:" + checkVisisble(Player.instance.gameObject));
        agent.SetDestination(Player.instance.transform.position);

        if (Input.GetKeyUp(KeyCode.Z))
        {
            character.infect();
        }
    }
}
