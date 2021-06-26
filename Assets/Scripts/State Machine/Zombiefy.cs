using UnityEngine;

namespace State_Machine
{
    public class Zombiefy : StateMachineBehaviour
    { 
        private float _zombiefyMaxTime;
        private float _zombiefyElapsedTime;

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
                animator.SetBool(AnimatorFields.Zombiefying, false);
                return;
            }

            _zombiefyElapsedTime += Time.deltaTime;
        }
    }
}
