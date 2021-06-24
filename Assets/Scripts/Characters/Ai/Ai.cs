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
    [NonSerialized] public bool rotating;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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

    public List<Vector3> getRotationPoints(int points)
    {
        List<Vector3> v3s = new List<Vector3>();
        for (int i = 0; i < points; i++)
        {
            float angle = i / (points * 1f) * 360f;
            v3s.Add(new Vector3((float)Math.Sin(angle), (float)Math.Cos(angle), 0));
        }

        return v3s;
    }
    
    public void rotate360()
    {
        rotating = true;
        StartCoroutine(RandRotationTime());
    }

    private IEnumerator RandRotationTime()
    {
        var r = new Random().Next(1, 5);
        Debug.Log($"r:{r}");
        yield return new WaitForSeconds(r);
        rotating = false;
    }
    public void StopRotating()
    {
        rotating = false;
    }

    private void rotateTowards(Vector3 targetPosition)
    {
        Vector3 dir = targetPosition - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    private void rotateTowardsVel()
    {
        if (agent.velocity != Vector3.zero)
        {
            Vector3 targetPosition = transform.position + agent.velocity.normalized;
            rotateTowards(targetPosition);
        }
    }

    private void doRotation()
    {
        // Debug.Log(rotating);

        if (rotating)
        {
            transform.RotateAround(transform.position, Vector3.forward, 100 * Time.deltaTime);
        }
        else
        {
            rotateTowardsVel();
        }
    }
    private void LateUpdate()
    {
        doRotation();
    }
    
    public override bool checkVisisble(GameObject go, float visionAngle=-1, float visionDistance=-1, List<string> layers = null)
    {
        if (go.GetComponent<Sauce>() != null)
        {
            layers.Add("obstacles");
        }
        if (go.GetComponent<Player>() != null)
        {//subject to cone of vision character stats
            float dist = Vector2.Distance(go.transform.position, transform.position);
            visionAngle = character.visionAngle;
            if (dist < 2) // very close by humans can see the player in a larger cone
            {
                visionAngle = 270;
            }
            return base.checkVisisble(go, visionAngle, character.visionDistance, layers);
        }
        
        //else is checking for another ai. they have 360 vision
        return base.checkVisisble(go, 360, character.visionDistance, layers);

    }
    
}
