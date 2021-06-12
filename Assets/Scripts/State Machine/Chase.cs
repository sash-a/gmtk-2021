using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : StateMachineBehaviour
{

    private Transform playerPos;
    private Controller controller;
    public float chaseSpeed = 10;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = Player.instance.transform;
        controller = animator.GetComponent<Controller>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform target = getClosestTarget(animator.transform.position);
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, target.position, chaseSpeed * Time.deltaTime);

        if (CharacterManager.getVisibleHorde(controller).Count == 0)
        {
            animator.SetBool("isPatrolling", true);
            animator.SetBool("isChasing", false);

        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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
