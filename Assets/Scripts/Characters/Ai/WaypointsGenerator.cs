using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WaypointsGenerator : MonoBehaviour
{
    private Controller controller;
    int layerMask = LayerMask.GetMask("obstacles", "wall");

    public void getCircleAroundPoint(Vector3 origin, int numPoints, float radius, float randomNess=0)
    {
        /*
         * randomness: [0,1], affects angle and radius randomness
         */

        float angleInc = 2 * Mathf.PI / numPoints;
        List<Vector3> circle = new List<Vector3>();
        for (int i = 0; i < numPoints; i++)
        {
            Vector3 candPoint = origin + Vector3.zero;
            candPoint[0] = Mathf.Sin(angleInc * i) * radius;
            candPoint[1] = Mathf.Cos(angleInc * i) * radius;

            if (isWaypointVisisble(circle[i]))
            {
                circle.Add(candPoint);
            }
        }

        if (circle.Count == 0)
        {
            throw new Exception("can't find any waypoints");
        }
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
