using Unity.VisualScripting;
using UnityEngine;

namespace State_Machine
{
    public class Chase : StateMachineBehaviour
    {
        private Ai _controller;
        private Character _character;

        private Vector3 _lastKnownPos;
        private GameObject _gameObject;

        private float _rotateTowardsThresh = 3;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _gameObject = animator.gameObject;

            _controller = animator.GetComponent<Ai>();
            _character = animator.GetComponent<Controller>().character;
            _controller.ClearAgentPath();
            if (_controller is Human && _controller.character.getInfectionFrac() == -1)
            { // non infected human
                _controller.visibilityIcon.setText("!");
            }
            else
            {
                _controller.visibilityIcon.setText("");
            }

            _rotateTowardsThresh = 3;
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

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _controller.autorotate = true;
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
            if (_controller == null)
            {
                _controller = _gameObject.GetComponent<Ai>();
            }

            if (Vector2.Distance(target, _gameObject.transform.position) < _rotateTowardsThresh)
            {
                _controller.autorotate = false;
                _controller.rotateTowards(target);
            }
            else
            {
                _controller.autorotate = true;
            }

            _controller.agent.SetDestination(target);
            Debug.DrawLine(_gameObject.transform.position, _controller.agent.destination, Color.red);
        }

        private bool TryAttack(Transform target)
        {
            if (_character is Attacker attacker && target)
            {
                float range = attacker.attackRange;
                bool targetIsSauce = target.GetComponent<Character>() is Sauce;
                if (targetIsSauce && _character is Melee)  // shorter range for smaller sauce
                {
                    //Debug.Log("melee attacking player. decreasing attack range");
                    range /= 2f;
                }

                var dist = Vector2.Distance(_gameObject.transform.position, target.position);
                if (dist < range && attacker.CheckCleanLineSight())
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
            //Debug.Log("in chase");
            var d = Vector2.Distance(_gameObject.transform.position, _lastKnownPos);
            if (CharacterManager.getVisibleOfInterest(_controller).Count == 0) // lost visibility
            {
                //Debug.Log("no targets");
                if (d < 1) // has arrived at last seen
                {
                    //Debug.Log("close to last seen");
                    animator.SetBool(AnimatorFields.Chasing, false);
                    animator.SetBool(AnimatorFields.Searching, true);
                }
                else
                {
                    GoTo(_lastKnownPos);

                    //Debug.Log("far away. last seen: " + _lastKnownPos + " at: " + _gameObject.transform.position);
                    if (_controller is Human && _controller.character.getInfectionFrac() == -1)
                    { // non infected human
                        _controller.visibilityIcon.setText("?");
                    }
                    else
                    {
                        _controller.visibilityIcon.setText("");
                    }
                }


                return true;
            }
            //Debug.Log("has targets");
            if (_controller is Human && _controller.character.getInfectionFrac() == -1)
            { // non infected human
                _controller.visibilityIcon.setText("!");
            }
            else
            {
                _controller.visibilityIcon.setText("");
            }
            return false;
        }
    }
}