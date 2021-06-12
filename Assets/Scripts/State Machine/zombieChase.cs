using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieChase : StateMachineBehaviour
{
    private Controller controller;
    public float chaseSpeed = 10;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<Controller>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform target = getClosestTarget(animator.transform.position);
        if (target)
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, target.position, chaseSpeed * Time.deltaTime);
        }

        if (CharacterManager.getVisibleHumans(controller).Count == 0)
        {
            animator.SetBool("isChasing", false);
        }

    }

    Transform getClosestTarget(Vector3 currentPos)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        foreach (Controller controller in CharacterManager.getVisibleHumans(controller))
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
