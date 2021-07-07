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

        private List<Vector3> searchPoints;
        private int _wayptIdx;
        private float _distThresh = 1f;
        private bool _initRoute;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _controller = animator.GetComponent<Ai>();

            _maxSearchTime = Random.Range(2, 5);
            _wayptIdx = 0;
            _searchTimePassed = 0f;
            _initRoute = true;

            if (_controller is Human && _controller.character.getInfectionFrac() == -1)
            {
                // non infected human
                _controller.visibilityIcon.setText("?");
            }
            else
            {
                _controller.visibilityIcon.setText("");
            }

            searchPoints =
                _controller.character.waypoints.wayGen.getCircleAroundPoint(Random.Range(2, 4), 3, 0.5f);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_controller == null)
            {
                _controller = animator.GetComponent<Ai>();
            }

            foreach (var point in searchPoints)
            {
                Debug.DrawLine(_controller.transform.position, point, Color.blue);
            }

            //Debug.Log("searching");
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

            // animator.transform.RotateAround(animator.transform.position, Vector3.forward, 100 * Time.deltaTime);
            WaypointPatrol(_controller.transform, _initRoute);
            _initRoute = false;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _controller.visibilityIcon.setText("");
        }

        // TODO this mostly a duplicate of Patrol.WaypointPatrol really should make it general
        void WaypointPatrol(Transform transform, bool initialRoute = false)
        {
            // if (_controller == null)
            // {
            //     _controller = _character.GetComponent<Ai>();
            // }

            if (Vector2.Distance(transform.position, searchPoints[_wayptIdx]) < _distThresh || initialRoute)
            {
                _wayptIdx++;
                _wayptIdx %= searchPoints.Count;
                _controller.agent.SetDestination(searchPoints[_wayptIdx]);
                // Debug.Log($"Waypoint set at :{_character.waypoints[_wayptIdx]}");
            }

            var position = _controller.transform.position;
            Debug.DrawLine(position, searchPoints[_wayptIdx], Color.green);
            try
            {
                Debug.DrawLine(position, _controller.agent.destination, Color.blue);
            }
            catch (Exception)
            {
                /* ignored */
            }
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