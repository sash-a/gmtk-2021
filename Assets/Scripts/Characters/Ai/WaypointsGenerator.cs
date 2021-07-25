using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WaypointsGenerator : MonoBehaviour
{
    private Controller controller;
    int layerMask = -1;

    /*
     * randomness: [0,1], affects angle and radius randomness
     */

    public static List<Vector3> getCircleAroundPoint(Controller controller, Vector3 center, int numPoints, float radius, float randomness, int layerMask)
    {
        float angleInc = 2 * Mathf.PI / numPoints;
        List<Vector3> circle = new List<Vector3>();
        int remainingTries = 3;
        for (int i = 0; i < numPoints; i++)
        {
            Vector3 candPoint = center + Vector3.zero;
            float angle = angleInc * i;
            float angleRandom = Random.Range(-angleInc, angleInc) * randomness;
            float radiusRandom = Random.Range(-radius, radius / 2) * randomness;

            candPoint[0] += Mathf.Sin(angle + angleRandom) * (radius + radiusRandom);
            candPoint[1] += Mathf.Cos(angle + angleRandom) * (radius + radiusRandom);

            if (isWaypointVisisbleAndReachable(controller, center, candPoint, layerMask))
            {
                circle.Add(candPoint);
            }
            else
            {
                if (randomness > 0 && remainingTries > 0) // non deterministic, try again
                {
                    i--; // send the loop back to the current point to try again
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

    private static bool isWaypointVisisbleAndReachable(Controller controller, Vector3 center, Vector3 waypoint, int layerMask)
    {
        // raycasts from current pos to waypoint, checks if any obstructions
        Vector2 diff = center - waypoint;
        RaycastHit2D hitInfo = Physics2D.Raycast(center, diff, diff.magnitude, layerMask);
        if (hitInfo)
        {
            return false;
        }

        return true;

        // Is visible, check if reachable
        var reachable = true;
        if (controller is Ai ai)
        {
            reachable = ai.agent.CalculatePath(waypoint, new NavMeshPath());
        }

        return reachable;
    }
}