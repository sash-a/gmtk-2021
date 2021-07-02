using UnityEngine;

namespace State_Machine
{
    public class Zombiefy : StateMachineBehaviour
    {
        private Ai controller;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (controller == null)
            {
                controller = animator.GetComponent<Ai>();
                controller.ClearAgentPath();
                controller.visibilityIcon.setText("");
            }
            // TODO rotate and make weird noises!
            if (!controller.character.isInfected())
            {
                animator.SetBool(AnimatorFields.Zombiefying, false);
                animator.SetBool(AnimatorFields.Patrolling, true);
            }
        }
    }
}
