using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace State_Machine
{
    public class Search : StateMachineBehaviour
    {
        private Ai _controller;
        private int _maxSearchTime;
        private float _searchTimePassed;

        private float _distThresh = 1f;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _controller = animator.GetComponent<Ai>();

            _maxSearchTime = Random.Range(3, 5);
            _searchTimePassed = 0f;

            if (_controller is Human && _controller.character.getInfectionFrac() == -1)
            {
                // non infected human
                _controller.visibilityIcon.setText("?");
            }
            else
            {
                _controller.visibilityIcon.setText("");
            }

            _controller.ClearAgentPath();
            _controller.agent.SetDestination(Player.instance.transform.position);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_controller == null)
            {
                _controller = animator.GetComponent<Ai>();
            }

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
            //Debug.Log("no targets");

            if (Vector2.Distance(animator.transform.position, _controller.agent.destination) < _distThresh)
            {
                animator.transform.RotateAround(animator.transform.position, Vector3.forward, 50 * Time.deltaTime);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _controller.visibilityIcon.setText("");
        }

        private void Chase(Animator anim)
        {
            _controller.ClearAgentPath();

            anim.SetBool(AnimatorFields.Chasing, true);

            anim.SetBool(AnimatorFields.Searching, false);
            anim.SetBool(AnimatorFields.Patrolling, false);

            anim.Play(nameof(Chase));
        }

        private void Patrol(Animator anim)
        {
            _controller.ClearAgentPath();

            anim.SetBool(AnimatorFields.Patrolling, true);

            anim.SetBool(AnimatorFields.Searching, false);
            anim.SetBool(AnimatorFields.Chasing, false);

            anim.Play(nameof(State_Machine.Patrol));
        }
    }
}