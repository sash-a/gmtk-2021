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
            print("hello");
            return false;
        }
        // print("within dist");
        float angle = Vector2.Angle(transform.right, target - pos);
        // Debug.Log("angle:" + angle + " forward: " +  transform.right + " diff: " + (target - pos) + " target: " + target + " pos: " + pos);
        if (angle > character.visionAngle / 2f)
        {
            return false;
        }
        
        // print("within angle");
        
        
        int layerMask = LayerMask.GetMask(LayerMask.LayerToName(go.layer));
        /*if (targetIsHuman)
        {
            layer_mask = LayerMask.GetMask("human");
        }
        else
        {
            layer_mask = LayerMask.GetMask("player",  "infected");
        }*/

        var hit = Physics2D.Raycast( pos, target - pos, character.visionDistance, layerMask);
        Debug.DrawLine(pos, target);
        Debug.DrawLine(pos, (Vector3)pos + transform.forward);
        
        print($"checking for collision with:{go.name} on layer {go.layer}");
        // print($"checking for collision with:{go.name}");
        
        if (hit.collider != null)
        {
            print($"collided with:{hit.collider.gameObject.name}");
            return hit.collider.gameObject == go;
        }
        Debug.Log("no collision");
        return false;
    }
}
