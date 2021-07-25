using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace State_Machine
{
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

        private bool isFollowing = false; // for zombie only
        private float zombiePickupDist = 2f;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _targetPatrolPoint = (Vector2) animator.transform.position + Random.insideUnitCircle * patrolRange;
            _controller = animator.GetComponent<Ai>();
            //Debug.Log($"ctrlr (strt):{_controller} {_controller == null}");
            _character = animator.GetComponent<Controller>().character;
            _gameObject = animator.gameObject;

            _controller.ClearAgentPath();
            _controller.visibilityIcon.setText("");
            _initPatrol = true;
            //Debug.Log("entering patrol");
            if (_controller is Zombie) // random patrol each time the zombie starts a new patrol
            {
                updateZombieWaypoints();
            }

            isFollowing = false; // zombies have bad memories. They forget you when distracted
        }

        public void updateZombieWaypoints()
        {
            if (_controller == null)
            {
                _controller = _gameObject.GetComponent<Ai>();
            }
            Vector3 centerPoint = Vector3.zero;
            if (ZombiePointManager.zombiePointsMap.ContainsKey(_controller))
            {
                ZombiePoint point = ZombiePointManager.zombiePointsMap[_controller];
                centerPoint = point.transform.position;
            }
            else
            {
                centerPoint = _controller.transform.position;
            }
                
            int layerMask = LayerMask.GetMask(new string[] {"wall", "obstacles"});
            List<Vector3> randomPatrol = WaypointsGenerator.getCircleAroundPoint(_controller, centerPoint, 7, 3, 0.6f,
                layerMask);
            Debug.Log("found " + randomPatrol.Count + "waypoints");
            _character.waypoints.useGeneratedWaypoints = true;
            _character.waypoints.setWaypoints(randomPatrol);
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
                //Vector3 target = Player.instance.transform.position;
                //float dist = Vector2.Distance(target, _gameObject.transform.position);
                //if (isFollowing || dist <= zombiePickupDist)
                //{
                //    isFollowing = true;
                //    FollowPatrol();
                //}
                //else
                //{
                //    WaypointPatrol(_gameObject.transform, _initPatrol);
                //}
                WaypointPatrol(_gameObject.transform, _initPatrol);

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
                _controller = _character.GetComponent<Ai>();
            }
            if (Vector2.Distance(transform.position, _character.waypoints[_wayptIdx]) < _distThresh || initialRoute)
            {
                _wayptIdx++;
                _wayptIdx %= _character.waypoints.Count;
            }
            _controller.agent.SetDestination(_character.waypoints[_wayptIdx]);


            var position = _gameObject.transform.position;
            Debug.DrawLine(position, _character.waypoints[_wayptIdx], Color.green);
            try
            {
                Debug.DrawLine(position, _controller.agent.destination, Color.blue);
            }
            catch (Exception)
            {
                /* ignored */
            }
        }

        void FollowPatrol()
        {
            Vector3 target = Player.instance.transform.position;
            var position = _gameObject.transform.position;

            float dist = Vector2.Distance(target, position);
            float followDist = 3f;

            _controller.agent.SetDestination(dist <= followDist ? position : target);
        }
    }
}