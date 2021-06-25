using UnityEngine;

namespace State_Machine
{
    public class AnimatorFields
    {
        public static readonly int Walking = Animator.StringToHash("walking");
        public static readonly int Attacking = Animator.StringToHash("Attacking");
        public static readonly int Zombiefying = Animator.StringToHash("zombiefying");
        public static readonly int Chasing = Animator.StringToHash("isChasing");
        public static readonly int Patrolling = Animator.StringToHash("isPatroling");
        public static readonly int Searching = Animator.StringToHash("isSearching");
    }
}