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
        character = GetComponent<Character>();
    }

    public void moveDirection(Vector2 dir)
    {
        rb.MovePosition(((Vector2)transform.position) + dir * (character.moveSpeed * Time.deltaTime));
    }

    void Attack()
    {
        // Get attack script
        Attacker attacker = GetComponent<Attacker>();

        // Check if attack component is attached
        if (attacker)
        {
            attacker.Attack();
        }
          
    }

    public bool checkVisisble(GameObject go)
    {
        var target = (Vector2)go.transform.position;
        var pos = (Vector2)transform.position;
        if (Vector2.Distance(target, pos) > character.visionDistance)
        {
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
        string myLayer = LayerMask.LayerToName(gameObject.layer);
        if (myLayer == "zombie")
        {
            layerMask = LayerMask.GetMask("human");
        }
        else if (myLayer == "human")
        {
            layerMask = LayerMask.GetMask("player",  "infected");
        }
        else
        {
            throw new Exception("unreckognised layer: " + myLayer);
        }

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
