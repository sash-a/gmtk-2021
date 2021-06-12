using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : StateMachineBehaviour
{

    public float patrolRange = 10;
    public float patrolSpeed = 5;
    private Vector2 targetPatrolPoint;
    private Ai controller;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        targetPatrolPoint = Random.insideUnitCircle * patrolRange;
        controller = animator.GetComponent<Ai>();
    } 

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // TODO: ADD PROPER PATROL LOGIC
        RandomPatrol(animator.transform);

        if (CharacterManager.getVisibleHorde(controller).Count > 0)
        {
            animator.SetBool("isChasing", true);
        }

    }

    void RandomPatrol(Transform transform)
    {
        if (Vector2.Distance(transform.position, targetPatrolPoint) > 0.2f)
        {
            //transform.position = Vector2.MoveTowards(transform.position, targetPatrolPoint, patrolSpeed * Time.deltaTime);
            controller.agent.SetDestination(targetPatrolPoint);
        }
        else
        {
            targetPatrolPoint = Random.insideUnitCircle * patrolRange;
        }
    }


}
