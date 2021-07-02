using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Rigidbody2D))]
public class Controller : MonoBehaviour
{
    [NonSerialized] public Character character;
    [NonSerialized] public Rigidbody2D rb;
    public SpriteRenderer offScreenIndicator;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Character[] characters = GetComponents<Character>();
        foreach (var ch in characters)
        {
            //Debug.Log(this + " found charcater: " + ch + " enabled: " + ch.enabled);
            if (ch.enabled)
            {
                character = ch;
            }
        }
    }

    public void moveDirection(Vector2 dir)
    {
        rb.MovePosition(((Vector2) transform.position) + dir * (character.moveSpeed * Time.deltaTime));
    }

    public virtual bool checkVisible(GameObject go, float visionAngle = -1, float visionDistance = -1,
        List<string> layers = null)
    {
        var target = (Vector2) go.transform.position;
        var pos = (Vector2) transform.position;
        if (visionDistance == -1)
        {
            throw new Exception();
        }

        if (visionAngle == -1)
        {
            throw new Exception();
        }

        float dist = Vector2.Distance(target, pos);
        if (dist > visionDistance)
        {
            return false;
        }

        float angle = Vector2.Angle(transform.right, target - pos);
        if (angle > visionAngle / 2f)
        {
            return false;
        }

        var hit = Physics2D.Raycast(pos, target - pos, visionDistance, LayerMask.GetMask(layers.ToArray()));
        // Debug.DrawLine(pos, target);
        Debug.DrawLine(pos, (Vector3) pos + transform.right * visionDistance);
        Debug.DrawLine(pos,
            (Vector3) pos + Quaternion.AngleAxis(visionAngle / 2f, transform.forward) * transform.right *
            visionDistance);
        Debug.DrawLine(pos,
            (Vector3) pos + Quaternion.AngleAxis(-visionAngle / 2f, transform.forward) * transform.right *
            visionDistance);

        if (hit.collider != null)
        {
            // Debug.Log(this + " hit: " + hit.collider.gameObject + " checking for: " + go + " eq: " + (hit.collider.gameObject == go));
            return hit.collider.gameObject == go;
        }

        return false;
    }
}