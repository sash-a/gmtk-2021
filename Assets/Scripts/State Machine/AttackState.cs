using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    private Ai controller;
    private Character _character;
    public float chaseSpeed = 10;

    private Vector3 lastKnownPos;
    private GameObject myGameObject;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Im attacking");
        myGameObject = animator.gameObject;

        controller = animator.GetComponent<Ai>();
        _character = animator.GetComponent<Character>();
        controller.ClearAgentPath();
    }

     // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.ClearAgentPath();
        if (_character is Ranged rangedChar)
        {
            // var dist = Vector2.Distance(myGameObject.transform.position, target.position);
            // if (dist > rangedChar.attackRange)
            // {
            //     
            // }
            // rangedChar.Attack();
            
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
