using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombiefy : StateMachineBehaviour
{
    private float _zombiefyMaxTime;
    private float _zombiefyElapsedTime;
    private static readonly int Zombiefying = Animator.StringToHash("zombiefying");

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Ai>().ClearAgentPath();
        _zombiefyElapsedTime = 0;
        _zombiefyMaxTime = 1;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // TODO rotate and make weird noises!
        if (_zombiefyElapsedTime > _zombiefyMaxTime)
        {
            animator.SetBool(Zombiefying, false);
            return;
        }

        _zombiefyElapsedTime += Time.deltaTime;
    }
}