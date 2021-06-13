using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class Ai : Controller
{
    [NonSerialized] public NavMeshAgent agent;
    public bool rotating;

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
        character.SpriteController.legsAnimator.SetBool("walking", agent.velocity.magnitude > 0);
        character.SpriteController.torsoAnimator.SetBool("walking", agent.velocity.magnitude > 0);
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

    public IEnumerator RandRotationTime()
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
}
