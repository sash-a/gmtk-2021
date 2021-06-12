using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai : Controller
{
    protected NavMeshAgent agent;
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void FixedUpdate()
    {
        character.SpriteController.legs.transform.rotation = Quaternion.LookRotation(transform.forward, agent.velocity);
        character.SpriteController.legsAnimator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
