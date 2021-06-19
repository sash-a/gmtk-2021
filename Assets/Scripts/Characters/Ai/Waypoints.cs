using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [NonSerialized] private List<Vector3> waypoints;
    public bool cyclical;
    
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
        waypoints = new List<Vector3>();
        Transform[] transes = GetComponentsInChildren<Transform>();
        for (int i = 1; i < transes.Length; i++) // first is the parent / ie this
        {
            waypoints.Add(transes[i].position);
        }
    }
}
