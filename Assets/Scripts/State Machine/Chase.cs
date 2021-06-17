using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Chase : StateMachineBehaviour
{

    private Ai controller;
    private Character _character;
    public float chaseSpeed = 10;

    private Vector3 lastKnownPos;
    private GameObject myGameObject;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Debug.Log("Im chasing");
        myGameObject = animator.gameObject;

        controller = animator.GetComponent<Ai>();
        _character = animator.GetComponent<Controller>().character;
        controller.ClearAgentPath();
    }

    private bool attacking = false;
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform target = getClosestTarget(animator.transform.position);
        if (target)
        {
            //animator.transform.position = Vector2.MoveTowards(animator.transform.position, target.position, chaseSpeed * Time.deltaTime);
            lastKnownPos = target.position;
        }
        
        if (controller == null)
        {
            controller = myGameObject.GetComponent<Ai>();
            if (controller == null)
            {
                throw new Exception("cannot get Ai from: " + myGameObject);
            }
        }

        if (controller.agent == null)
        {
            controller.agent = controller.GetComponent<NavMeshAgent>();
        }

        if (_character is Attacker attacker && target)
        {
            var dist = Vector2.Distance(myGameObject.transform.position, target.position);
            if (dist < attacker.attackRange && attacker.checkCleanLineSight())
            {
                attacking = true;
                Debug.Log("ATTACKING!!!!");
                controller.ClearAgentPath();
                attacker.Attack();
            }
            else
            {
                // Debug.Log("Chasing");
                try
                {
                    controller.agent.SetDestination(lastKnownPos);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return;
                }

                Debug.DrawLine(animator.transform.position, controller.agent.destination, Color.red);
            }
        }

        if (!attacking)
        {
            controller.agent.SetDestination(lastKnownPos);
            Debug.DrawLine(animator.transform.position, controller.agent.destination, Color.red);
        }
        // Debug.Log($"Lastknow:{lastKnownPos}");
        
        // Debug.DrawLine(animator.transform.position, lastKnownPos, Color.blue);

        var d = Vector2.Distance(animator.transform.position, lastKnownPos);
        if (CharacterManager.getVisibleOfInterest(controller).Count == 0 && d < 1)
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isPatroling", true);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // controller.ClearAgentPath();
    }

    Transform getClosestTarget(Vector3 currentPos)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        if (controller == null)
        {
            controller = myGameObject.GetComponent<Ai>();
            if (controller == null)
            {
                throw new Exception("cannot get Ai from: " + myGameObject);
            }
        }

        foreach (Controller controller in CharacterManager.getVisibleOfInterest(controller))
        {
            float dist = Vector3.Distance(controller.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = controller.transform;
                minDist = dist;
            }
        }

        return tMin;

    }


}
