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
    [NonSerialized] public CharacterGlowEffect glowEffect;
    
    [NonSerialized] public float glowTimeLeft = 0;
    public static float glowTime = 0.25f;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Character[] characters = GetComponents<Character>();
        foreach (var ch in characters)
        {
            Debug.Log(this + " found charcater: " + ch + " enabled: " + ch.enabled);
            if (ch.enabled)
            {
                character = ch;
            }
        }
        
        glowEffect = GetComponentInChildren<CharacterGlowEffect>();
        if (glowEffect == null)
        {
            throw new Exception("no glow effect on character");
        }
        glowEffect.gameObject.SetActive(false);
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

    public bool checkVisisble(GameObject go, float visionAngle=-1, float visionDistance=-1)
    {
        var target = (Vector2)go.transform.position;
        var pos = (Vector2)transform.position;
        if (visionDistance == -1)
        {
            visionDistance = character.visionDistance;
        }

        if (visionAngle == -1)
        {
            visionAngle = character.visionAngle;
        }

        float dist = Vector2.Distance(target, pos);
        if (dist > visionDistance)
        {
            return false;
        }

        if (dist < 2) // very close by enemies are visible in a larger cone
        {
            visionAngle = 270;
        }

        float angle = Vector2.Angle(transform.right, target - pos);
        if (angle > visionAngle / 2f)
        {
            return false;
        }

        int layerMask = LayerMask.GetMask(LayerMask.LayerToName(go.layer));
        string myLayer = LayerMask.LayerToName(gameObject.layer);
        if (myLayer == "zombie" || myLayer == "player")
        {
            layerMask = LayerMask.GetMask("human", "wall");
        }
        else if (myLayer == "human")
        {
            if (go.GetComponent<Sauce>() != null) // is searching for sauce. should not be able to see over obstacles
            {
                layerMask = LayerMask.GetMask("player", "infected", "wall", "zombie", "obstacles");
            }
            else
            {
                layerMask = LayerMask.GetMask("player", "infected", "wall", "zombie");
            }
        }
        else
        {
            throw new Exception("unreckognised layer: " + myLayer);
        }

        var hit = Physics2D.Raycast( pos, target - pos, visionDistance, layerMask);
        // Debug.DrawLine(pos, target);
        Debug.DrawLine(pos, (Vector3)pos + transform.right * visionDistance);
        Debug.DrawLine(pos, (Vector3)pos + Quaternion.AngleAxis(visionAngle / 2f, transform.forward) * transform.right * visionDistance);
        Debug.DrawLine(pos, (Vector3)pos + Quaternion.AngleAxis(-visionAngle / 2f, transform.forward) * transform.right * visionDistance);

        if (hit.collider != null)
        {
            // Debug.Log(this + " hit: " + hit.collider.gameObject + " checking for: " + go + " eq: " + (hit.collider.gameObject == go));
            return hit.collider.gameObject == go;
        }
        return false;
    }

    public void glow()
    {
        glowTimeLeft = glowTime;
        if (! glowEffect.gameObject.activeSelf) // not currently glowing
        {
            glowTimeLeft = glowTime;
            glowEffect.gameObject.SetActive(true);
            StartCoroutine(glowForTime());
        }
    }
    
    private IEnumerator glowForTime()
    {
        while (glowTimeLeft > 0)
        {
            yield return new WaitForSeconds(0.05f);
            glowTimeLeft -= 0.05f;
        }
        glowEffect.gameObject.SetActive(false);
    }
}
