using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Chase : StateMachineBehaviour
{
    private Ai _controller;
    private Character _character;

    private Vector3 _lastKnownPos;
    private GameObject _gameObject;

    private bool _attacking;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _gameObject = animator.gameObject;

        _controller = animator.GetComponent<Ai>();
        _character = animator.GetComponent<Controller>().character;
        _controller.ClearAgentPath();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // TODO maybe should linger on old target a bit
        Transform target = GetClosestTarget(animator.transform.position);
        if (target)
        {
            _lastKnownPos = target.position;
        }

        if (TryAttack(target)) // if attacking don't need to check anything else
        {
            return;
        }

        if (CheckAndSwitchStates(animator)) // if switching states don't also add to destination
        {
            return;
        }

        GoTo(_lastKnownPos);
    }

    Transform GetClosestTarget(Vector3 currentPos)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        if (_controller == null)
        {
            _controller = _gameObject.GetComponent<Ai>();
        }

        foreach (Controller controller in CharacterManager.getVisibleOfInterest(_controller))
        {
            float dist = Vector3.Distance(controller.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = controller.transform;
                minDist = dist;
            }
        }

        return tMin;
    }

    private void GoTo(Vector3 target)
    {
        _controller.agent.SetDestination(target);
        Debug.DrawLine(_gameObject.transform.position, _controller.agent.destination, Color.red);
    }

    private bool TryAttack(Transform target)
    {
        if (_character is Attacker attacker && target)
        {
            var dist = Vector2.Distance(_gameObject.transform.position, target.position);
            if (dist < attacker.attackRange && attacker.checkCleanLineSight())
            {
                _controller.ClearAgentPath();
                attacker.Attack();
                return true;
            }
        }

        return false;
    }

    private bool CheckAndSwitchStates(Animator animator)
    {
        var d = Vector2.Distance(_gameObject.transform.position, _lastKnownPos);
        if (CharacterManager.getVisibleOfInterest(_controller).Count == 0 && d < 1)
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isPatroling", true);

            return true;
        }


        return false;
    }
}