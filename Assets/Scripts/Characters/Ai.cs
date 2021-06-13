using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai : Controller
{
    [NonSerialized] public NavMeshAgent agent;
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void ClearAgentPath()
    {
        agent.isStopped = true;
        agent.ResetPath();   
    }

    public void FixedUpdate()
    {
        character.SpriteController.legs.transform.rotation = Quaternion.LookRotation(transform.right, agent.velocity);
        character.SpriteController.legsAnimator.SetBool("walking", agent.velocity.magnitude > 0);
    }
}
