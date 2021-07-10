using UnityEngine;

namespace State_Machine
{
    public class Zombiefy : StateMachineBehaviour
    {
        private Ai controller;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (controller == null)
            {
                controller = animator.GetComponent<Ai>();
                controller.ClearAgentPath();
                controller.visibilityIcon.setText("");
            }        
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
            if (controller == null)
            {
                controller = animator.GetComponent<Ai>();
            }

            if (controller is Zombie)
            {
                //Debug.Log("turning into zombie. Ctrl: " + animator.gameObject.GetComponent<Controller>() + " inf frac: " + controller.character.getInfectionFrac());
                animator.SetBool(AnimatorFields.Zombiefying, false);
                animator.SetBool(AnimatorFields.Patrolling, true);  
            }
        }
    }
}
