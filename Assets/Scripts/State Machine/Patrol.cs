using System;
using State_Machine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Patrol : StateMachineBehaviour
{
    public float patrolRange = 1;
    private Vector2 _targetPatrolPoint;

    private Ai _controller;
    private Character _character;
    private GameObject _gameObject;

    private int _wayptIdx;
    private float _timeSinceLastSeen;
    private Vector3 _lastKnownPos;
    private bool _initPatrol;
    private float _distThresh = 1;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _targetPatrolPoint = (Vector2) animator.transform.position + Random.insideUnitCircle * patrolRange;
        _controller = animator.GetComponent<Ai>();
        //Debug.Log($"ctrlr (strt):{_controller} {_controller == null}");
        _character = animator.GetComponent<Controller>().character;
        _gameObject = animator.gameObject;

        _controller.ClearAgentPath();

        _initPatrol = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // controller is only ever null on player ejected (controller changes from `Player` to `AI`
        // because for some reason start isn't called on animator.enabled = true
        if (_controller == null)
        {
            _controller = animator.GetComponent<Ai>();
        }
        
        if (CharacterManager.getVisibleOfInterest(_controller).Count > 0)
        {
            animator.SetBool(AnimatorFields.Chasing, true);
            animator.SetBool(AnimatorFields.Patrolling, false);
            return;
        }

        DoPatrol();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _controller.ClearAgentPath();
    }

    private void DoPatrol()
    {
        if (_controller is Zombie)
        {
            RandomPatrol(_gameObject.transform, _initPatrol);
        }
        else
        {
            WaypointPatrol(_gameObject.transform, _initPatrol);
        }

        _initPatrol = false;
    }

    void WaypointPatrol(Transform transform, bool initialRoute = false)
    {
        if (_controller == null)
        {
            _controller  = _character.GetComponent<Ai>();
        }

        if (Vector2.Distance(transform.position, _character.waypoints[_wayptIdx]) < _distThresh || initialRoute)
        {
            _wayptIdx++;
            _wayptIdx %= _character.waypoints.Count;
            _controller.agent.SetDestination(_character.waypoints[_wayptIdx]);
            // Debug.Log($"Waypoint set at :{_character.waypoints[_wayptIdx]}");
        }

        var position = _gameObject.transform.position;
        Debug.DrawLine(position, _character.waypoints[_wayptIdx], Color.green);
        try
        {
            Debug.DrawLine(position, _controller.agent.destination, Color.blue);
        }catch(Exception e){}
    }

    void RandomPatrol(Transform transform, bool initialRoute = false)
    {
        var position = transform.position;


        if (Vector2.Distance(position, _targetPatrolPoint) < _distThresh || initialRoute)
        {
            _targetPatrolPoint = (Vector2) position + Random.insideUnitCircle * patrolRange;
            _controller.agent.SetDestination(_targetPatrolPoint);
        }

        Debug.DrawLine(position, _targetPatrolPoint, Color.blue);
    }
}