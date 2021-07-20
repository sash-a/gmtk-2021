using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityManager : MonoBehaviour
{
    public static bool canLookerSeeObject(GameObject looker, GameObject obj, float visionAngle = -1, float visionDistance = -1,
        List<string> layers = null)
    {
        var target = (Vector2) obj.transform.position;
        var pos = (Vector2) looker.transform.position;
        if (visionDistance == -1)
        {
            throw new Exception();
        }

        if (visionAngle == -1)
        {
            throw new Exception();
        }

        float dist = Vector2.Distance(target, pos);
        if (dist > visionDistance)  // too far away
        {
            return false;
        }

        float angle = Vector2.Angle(looker.transform.right, target - pos);
        if (angle > visionAngle / 2f)  // out of peripheral sight
        {
            return false;
        }
        
        Debug.DrawLine(pos, (Vector3) pos + looker.transform.right * visionDistance);
        Debug.DrawLine(pos,
            (Vector3) pos + Quaternion.AngleAxis(visionAngle / 2f, looker.transform.forward) * looker.transform.right *
            visionDistance);
        Debug.DrawLine(pos,
            (Vector3) pos + Quaternion.AngleAxis(-visionAngle / 2f, looker.transform.forward) * looker.transform.right *
            visionDistance);

        var hit = Physics2D.Raycast(pos, target - pos, visionDistance, LayerMask.GetMask(layers.ToArray()));
        if (hit.collider != null)
        {
            // Debug.Log(this + " hit: " + hit.collider.gameObject + " checking for: " + go + " eq: " + (hit.collider.gameObject == go));
            return hit.collider.gameObject == obj;
        }

        return false;
    }
}
