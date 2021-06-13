using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : StateMachineBehaviour
{

    public float patrolRange = 1;
    public float patrolSpeed = 5;
    private Vector2 targetPatrolPoint;
    private Ai controller;
    private Character _character;

    private int wayptIdx;
    private float timeSinceLastSeen;
    private Vector3 lastKnownPos;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        targetPatrolPoint = (Vector2) animator.transform.position + Random.insideUnitCircle * patrolRange;
        controller = animator.GetComponent<Ai>();
        _character = animator.GetComponent<Character>();
        // controller.ClearAgentPath();
        controller.agent.SetDestination(_character.waypoints[wayptIdx]);
    } 

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // TODO: ADD PROPER PATROL LOGIC
        // RandomPatrol(animator.transform);

        if (controller == null) {
            controller = animator.GetComponent<Ai>();
            if (controller == null)
            {
                throw new System.Exception("Null controller in patrol");
            }
        }

        if (controller is Zombie)
        {
            RandomPatrol(animator.transform);
        }
        else
        {
            WaypointPatrol(animator.transform);
        }
        
        Debug.DrawLine(animator.transform.position, _character.waypoints[wayptIdx], Color.green);
        Debug.DrawLine(animator.transform.position, controller.agent.destination, Color.blue);
        
        if (CharacterManager.getVisibleOfInterest(controller).Count > 0)
        {
            animator.SetBool("isChasing", true);
            animator.SetBool("isPatroling", false);
            // Debug.Log("exiting patrol");
        }

    }

    void WaypointPatrol(Transform transform)
    {
        if (Vector2.Distance(transform.position, _character.waypoints[wayptIdx]) < 1)
        {
            wayptIdx++;
            wayptIdx %= _character.waypoints.Count;
            controller.agent.SetDestination(_character.waypoints[wayptIdx]);
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // controller.ClearAgentPath();
    }
    
    void RandomPatrol(Transform transform)
    {
        if (controller == null) {
            controller = _character.GetComponent<Ai>();
            if (controller == null)
            {
                throw new System.Exception("Null controller in patrol");
            }
        }
        if (controller.agent == null)
        {
            controller.agent = controller.GetComponent<NavMeshAgent>();
        }
        if (Vector2.Distance(transform.position, targetPatrolPoint) > 3f || controller.agent.velocity.magnitude < 1)
        {
            //transform.position = Vector2.MoveTowards(transform.position, targetPatrolPoint, patrolSpeed * Time.deltaTime);
            controller.agent.SetDestination(targetPatrolPoint);
            Debug.DrawLine(transform.position, targetPatrolPoint);
        }
        else
        {
            targetPatrolPoint = (Vector2) transform.position + Random.insideUnitCircle * patrolRange;
        }
    }


}
