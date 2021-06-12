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


    public float glowTimeLeft = 0;
    public static float glowTime = 0.25f;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
        glowEffect = GetComponentInChildren<CharacterGlowEffect>();
        if (glowEffect == null)
        {
            throw new Exception("no glow effect on character");
        }
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

        float angle = Vector2.Angle(transform.right, target - pos);
        if (angle > character.visionAngle / 2f)
        {
            return false;
        }

        int layerMask = LayerMask.GetMask(LayerMask.LayerToName(go.layer));
        string myLayer = LayerMask.LayerToName(gameObject.layer);
        if (myLayer == "zombie" || myLayer == "player")
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

        if (hit.collider != null)
        {
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
