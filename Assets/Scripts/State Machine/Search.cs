using UnityEngine;
using Random = UnityEngine.Random;

namespace State_Machine
{
    public class Search : StateMachineBehaviour
    {
        private Ai _controller;
        private int _maxSearchTime;
        private float _searchTimePassed;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _controller = animator.GetComponent<Ai>();

            _maxSearchTime = Random.Range(2, 5);
            _searchTimePassed = 0f;
            _controller.visibilityIcon.setText(_controller is Zombie ? "" : "?");
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

            animator.transform.RotateAround(animator.transform.position, Vector3.forward, 100 * Time.deltaTime);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _controller.visibilityIcon.setText("");
        }

        private void Chase(Animator anim)
        {
            anim.SetBool(AnimatorFields.Chasing, true);

            anim.SetBool(AnimatorFields.Searching, false);
            anim.SetBool(AnimatorFields.Patrolling, false);

            anim.Play(nameof(Chase));
        }

        private void Patrol(Animator anim)
        {
            anim.SetBool(AnimatorFields.Patrolling, true);

            anim.SetBool(AnimatorFields.Searching, false);
            anim.SetBool(AnimatorFields.Chasing, false);

            anim.Play(nameof(State_Machine.Patrol));
        }
    }
}