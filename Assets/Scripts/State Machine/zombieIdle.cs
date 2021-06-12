using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieIdle : StateMachineBehaviour
{
    private Controller controller;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<Controller>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (CharacterManager.getVisibleHumans(controller).Count > 0)
        {
            animator.SetBool("isChasing", true);
        }

    }

}
