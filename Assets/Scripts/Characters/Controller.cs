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
        return VisibilityManager.canLookerSeeObject(gameObject, go, visionAngle, visionAngle, layers);
    }
}