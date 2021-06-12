using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : StateMachineBehaviour
{

    public float patrolRange = 10;
    public float patrolSpeed = 5;
    private Vector2 targetPatrolPoint;
    private Controller controller;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        targetPatrolPoint = Random.insideUnitCircle * patrolRange;
    } 

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // TODO: ADD PATROl LOGIC
        RandomPatrol(animator.transform);

       
        if (CharacterManager.getVisibleHorde(controller).Count > 0)
        {
            animator.SetBool("isPatrolling", false);
            animator.SetBool("isChasing", true);

        }

    }

    void RandomPatrol(Transform transform)
    {
        if (Vector2.Distance(transform.position, targetPatrolPoint) > 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPatrolPoint, patrolSpeed * Time.deltaTime);
        }
        else
        {
            targetPatrolPoint = Random.insideUnitCircle * patrolRange;
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    

}
