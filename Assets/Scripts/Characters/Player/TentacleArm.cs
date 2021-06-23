using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleArm : MonoBehaviour
{
    private float baseLegth;
    // Start is called before the first frame update
    void Start()
    {
        baseLegth = 1f / transform.parent.localScale.x;
        //Debug.Log("base len:" + baseLegth + " parent scale: " + transform.parent.localScale);
    }

    public void setMouseDistance(float dist)
    {
        //Debug.Log("mousedist: " + dist + " tentx:  " + baseLegth * dist);
        transform.localScale = new Vector3(baseLegth * dist, 1, 1);
    }
}
