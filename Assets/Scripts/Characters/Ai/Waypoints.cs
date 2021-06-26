using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [NonSerialized] private List<Vector3> waypoints;
    public bool cyclical;
    [NonSerialized] public WaypointsGenerator wayGen;
    public bool useGeneratedWaypoints = false;

    private void Awake()
    {
        wayGen = GetComponent<WaypointsGenerator>();
    }

    public void setWaypoints(List<Vector3> points)
    {
        waypoints = points;
    }

    public Vector3 this[int index]
    {
        get => waypoints[index % waypoints.Count];
    }

    public int Count
    {
        get
        {
            if (cyclical)
            {
                return int.MaxValue;
            }
            else
            {
                return waypoints.Count;
            }
        }
    }

    public void compute()
    {
        if (useGeneratedWaypoints)
        { // no need to read these gameobjects, we will generate some with code instead
            return;
        }
        waypoints = new List<Vector3>();
        Transform[] transes = GetComponentsInChildren<Transform>();
        for (int i = 1; i < transes.Length; i++) // first is the parent / ie this
        {
            waypoints.Add(transes[i].position);
        }
    }
}
