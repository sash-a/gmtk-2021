using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleArm : MonoBehaviour
{
    private float baseLegth;
    [NonSerialized] public float armLength;
    public Transform tip;
    void Start()
    {
        baseLegth = 1f / transform.parent.localScale.x;
        //Debug.Log("base len:" + baseLegth + " parent scale: " + transform.parent.localScale);
    }

    public void updateLength()
    { // raycast to possibly cut the tentancle short
        int layerMask = LayerMask.GetMask("human", "wall", "obstacles");
        var hit = Physics2D.Raycast( transform.position, transform.right, armLength, layerMask);
        if (hit.collider != null)
        {
            armLength = Mathf.Min(armLength, hit.distance);
        }
        transform.localScale = new Vector3(baseLegth * armLength, 1, 1);
    }
}
