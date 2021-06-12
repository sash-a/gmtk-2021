using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float moveSpeed;
    public float visionDistance;
    public float visionAngle;
    public float infectionTime;

    public abstract void die();
}
