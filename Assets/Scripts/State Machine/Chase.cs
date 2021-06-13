using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chase : StateMachineBehaviour
{

    private Ai controller;
    public float chaseSpeed = 10;

    private Vector3 lastKnownPos;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Im chasing");

        controller = animator.GetComponent<Ai>();
        controller.ClearAgentPath();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform target = getClosestTarget(animator.transform.position);
        if (target)
        {
            //animator.transform.position = Vector2.MoveTowards(animator.transform.position, target.position, chaseSpeed * Time.deltaTime);
            lastKnownPos = target.position;
        }

        controller.agent.SetDestination(lastKnownPos);
        
        // Debug.Log($"Lastknow:{lastKnownPos}");
        
        Debug.DrawLine(animator.transform.position, controller.agent.destination, Color.red);
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
        foreach (Controller controller in CharacterManager.getVisibleHorde(controller))
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
