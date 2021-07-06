using System;
using System.Collections;
using System.Collections.Generic;
using State_Machine;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class Ai : Controller
{
    [NonSerialized] public NavMeshAgent agent;
    [NonSerialized] public VisibilityIcon visibilityIcon;
    [NonSerialized] public bool autorotate;

    public static float closebyVisionAngle = 200;

    public void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        autorotate = true;
    }

    public void Start()
    {
        GameObject vis = Instantiate(UIManager.instance.visibilityIconPrefab);
        visibilityIcon = vis.GetComponent<VisibilityIcon>();
        visibilityIcon.controller = this;
    }

    public void ClearAgentPath()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void FixedUpdate()
    {
        // character.SpriteController.legs.transform.rotation = Quaternion.LookRotation(transform.right, agent.velocity);
        character.SpriteController.legsAnimator.SetBool(AnimatorFields.Walking, agent.velocity.magnitude > 0);
        character.SpriteController.torsoAnimator.SetBool(AnimatorFields.Walking, agent.velocity.magnitude > 0);
    }

    private void LateUpdate()
    {
        doRotation();
    }

    public void rotateTowards(Vector3 targetPosition)
    {
        Vector3 dir = targetPosition - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void rotateTowardsVel()
    {
        if (agent.velocity != Vector3.zero && autorotate)
        {
            Vector3 targetPosition = transform.position + agent.velocity.normalized;
            rotateTowards(targetPosition);
        }
    }

    private void doRotation()
    {
        if (agent.velocity != Vector3.zero)
        {
            rotateTowardsVel();
        }
    }

    public override bool checkVisible(GameObject go, float visionAngle = -1, float visionDistance = -1,
        List<string> layers = null)
    {
        Player player = go.GetComponent<Player>();
        if (player != null)  // go is the player
        {
            Character character = go.GetComponent<Character>();
            if (character is Sauce) // if the player is not in a host, they cannot be seen behind obstacles
            {
                layers.Add("obstacles");
            }
            else
            { // for a small time when entering a host, obstacles continue to hide you
                if (player.timeOfCharacterChange - Time.time < 0.05f)
                {
                    layers.Add("obstacles");
                }
            }
            
            //subject to cone of vision character stats
            float dist = Vector2.Distance(go.transform.position, transform.position);
            visionAngle = character.visionAngle;
            if (dist < 2) // very close by humans can see the player in a larger cone
            {
                visionAngle = closebyVisionAngle;
            }

            return base.checkVisible(go, visionAngle, character.visionDistance, layers);
        }

        //else is checking for another ai. they have 360 vision
        float Ai2AiVision = 180;
        if (((Attacker) character).playerState.GetBool(AnimatorFields.Chasing))
        { // if I am chasing, i am alert, so I have a full FOV
            Ai2AiVision = 360;
        }
        return base.checkVisible(go, Ai2AiVision, character.visionDistance, layers);
    }
}