using System;
using System.Collections;
using System.Collections.Generic;
using State_Machine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Search : StateMachineBehaviour
{
    private Ai _controller;
    private int _maxSearchTime;
    private float _searchTimePassed;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _controller = animator.GetComponent<Ai>();
        _controller.autoRotate = false;

        _maxSearchTime = Random.Range(2, 5);
        _searchTimePassed = 0f;
        _controller.visibilityIcon.setText(_controller is Zombie ? "" : "?");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _searchTimePassed += Time.deltaTime;
        if (_searchTimePassed > _maxSearchTime)
        {
            Patrol(animator);
            return;
        }

        if (CharacterManager.getVisibleOfInterest(_controller).Count > 0)
        {
            Chase(animator);
            return;
        }

        animator.transform.RotateAround(animator.transform.position, Vector3.forward, 100 * Time.deltaTime);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _controller.autoRotate = true;
        _controller.visibilityIcon.setText("");
    }

    private void Chase(Animator anim)
    {
        anim.SetBool(AnimatorFields.Chasing, true);

        anim.SetBool(AnimatorFields.Searching, false);
        anim.SetBool(AnimatorFields.Patrolling, false);

        anim.Play("Chase");
    }

    private void Patrol(Animator anim)
    {
        anim.SetBool(AnimatorFields.Patrolling, true);

        anim.SetBool(AnimatorFields.Searching, false);
        anim.SetBool(AnimatorFields.Chasing, false);

        anim.Play("Patrol");
    }
}