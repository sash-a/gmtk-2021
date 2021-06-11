using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai : Controller
{
    public bool checkVisisble(GameObject go)
    {
        var target = (Vector2)go.transform.position;
        var pos = (Vector2)transform.position;
        if (Vector2.Distance(target, pos) > character.visionDistance)
        {
            return false;
        }
        print("within dist");
        float angle = Vector2.Angle(transform.right, target - pos);
        Debug.Log("angle:" + angle + " forward: " +  transform.right + " diff: " + (target - pos) + " target: " + target + " pos: " + pos);
        if (angle > character.visionAngle / 2f)
        {
            return false;
        }
        
        print("within angle");

        var hit = Physics2D.Raycast( pos, target - pos, character.visionDistance, go.layer);
        Debug.DrawLine(pos, target);
        Debug.DrawLine(pos, (Vector3)pos + transform.forward);
        if (hit.collider != null)
        {
            return hit.collider.gameObject == go;
        }

        return false;
    }
}
