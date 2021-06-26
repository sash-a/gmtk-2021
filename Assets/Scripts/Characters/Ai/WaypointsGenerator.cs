using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaypointsGenerator : MonoBehaviour
{
    private Controller controller;
    int layerMask = -1;

    public List<Vector3> getCircleAroundPoint(int numPoints, float radius, float randomNess=0)
    {
        /*
         * randomness: [0,1], affects angle and radius randomness
         */
        if (layerMask == -1)
        {
            layerMask = LayerMask.GetMask("obstacles", "wall");
        }

        float angleInc = 2 * Mathf.PI / numPoints;
        List<Vector3> circle = new List<Vector3>();
        int remainingTries = 3;
        for (int i = 0; i < numPoints; i++)
        {
            Vector3 candPoint = transform.position + Vector3.zero;
            float angle = angleInc * i;
            float angleRandom = Random.Range(-angleInc, angleInc) * randomNess;
            float radiusRandom = Random.Range(-radius, radius/2) * randomNess;

            candPoint[0] += Mathf.Sin(angle + angleRandom) * (radius + radiusRandom);
            candPoint[1] += Mathf.Cos(angle + angleRandom) * (radius + radiusRandom);

            if (isWaypointVisisble(candPoint)) 
            {
                circle.Add(candPoint);
            }
            else
            {
                if (randomNess > 0 && remainingTries > 0) // non deterministic, try again
                {
                    i --; // send the loop back to the current point to try again
                    remainingTries--;
                }
                else
                {
                    remainingTries = 3;
                }
            }
        }

        if (circle.Count == 0)
        {
            throw new Exception("can't find any waypoints");
        }

        //print("found " + circle.Count + "/" + numPoints+ " waypoints");
        return circle;
    }

    private bool isWaypointVisisble(Vector3 waypoint)
    { // raycasts from current pos to waypoint, checks if any obstructions
        Vector2 diff = transform.position - waypoint;
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, diff, diff.magnitude, layerMask);
        if (hitInfo)
        {
            return false;
        }

        return true;
    }
}
