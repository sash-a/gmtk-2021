using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityManager : MonoBehaviour
{
    public static bool canLookerSeeObject(GameObject looker, GameObject obj, float visionAngle = -1, float visionDistance = -1,
        List<string> layers = null)
    {
        return canLookerSeeObject(looker.transform, obj, visionAngle, visionDistance, layers);
    }
    
    public static bool canLookerSeeObject(Transform looker, GameObject obj, float visionAngle = -1, float visionDistance = -1,
        List<string> layers = null)
    {
        var target = (Vector2) obj.transform.position;
        var pos = (Vector2) looker.position;
        if (visionDistance == -1)
        {
            throw new Exception("must provide vision distance");
        }

        if (layers == null)
        {
            throw new Exception("must provide collidable layers");
        }

        if (visionAngle == -1)
        {
            throw new Exception("must provide vision angle");
        }

        float dist = Vector2.Distance(target, pos);
        if (dist > visionDistance)  // too far away
        {
            return false;
        }

        float angle = Vector2.Angle(looker.right, target - pos);
        if (angle > visionAngle / 2f)  // out of peripheral sight
        {
            return false;
        }
        
        Debug.DrawLine(pos, (Vector3) pos + looker.right * visionDistance);
        Debug.DrawLine(pos,
            (Vector3) pos + Quaternion.AngleAxis(visionAngle / 2f, looker.forward) * looker.right *
            visionDistance);
        Debug.DrawLine(pos,
            (Vector3) pos + Quaternion.AngleAxis(-visionAngle / 2f, looker.forward) * looker.right *
            visionDistance);

        var hit = Physics2D.Raycast(pos, target - pos, visionDistance, LayerMask.GetMask(layers.ToArray()));
        if (hit.collider != null)
        {
            // Debug.Log(this + " hit: " + hit.collider.gameObject + " checking for: " + go + " eq: " + (hit.collider.gameObject == go));
            return hit.collider.gameObject == obj;
        }

        return false;
    }

    public static bool isLineClear(Vector3 start, Vector3 end, List<string> layers = null)
    {
        Vector3 dir = end - start;
        var hit = Physics2D.Raycast(start, dir, dir.magnitude, LayerMask.GetMask(layers.ToArray()));
        return hit.collider == null;
    }

    public static bool isPlayerEffectivelyVisible(Controller looker) // includes logic around when player can be seen/ is incognito
    {
        if (Player.instance.character is Sauce)
        {
            if (Player.instance.remainingSlideTime <= 0) // done sliding
            {
                if (looker.checkVisible(Player.instance.gameObject))
                {
                    return true;
                }
            }
        }
        else
        {
            // is in human
            if (((Human) looker).sussPeople.Contains(Player.instance.character))
            {
                // this human has seen this host do something suss
                if (looker.checkVisible(Player.instance.gameObject)) // is suspicious and in line of sight
                {  // once sussed, always recognisable
                    return true;
                }
            }

            if (Time.time - Player.instance.character.lastShotTime < Character.shotSoundTime &&
                Player.instance.character.lastShotTime != -1)
            {
                //human has shot very recently. we will add them to nearby humans suss list
                float distance = Vector2.Distance(Player.instance.transform.position, looker.transform.position);
                if (distance < Character.shotSoundDistance)
                {
                    // looker is close enough to hear the gun shot
                    if (looker.checkVisible(Player.instance.gameObject, visionAngle:360))
                    {  // clean line of sight to the shooter, they are added to the suss list
                        ((Human) looker).sussPeople.Add(Player.instance.character);
                    }
                    return true;
                }
            }
        }

        return false;
    }
}
