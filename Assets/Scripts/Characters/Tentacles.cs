using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacles : MonoBehaviour
{
    private Color startColour = new Color(255, 100, 0, 0.15f);
    private SpriteRenderer[] sprites; 
    private void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in sprites)
        {
            sprite.color = startColour;
        }
    }

    public void makeZombie()
    {
        foreach (var sprite in sprites)
        {
            sprite.color = Color.white;
        }
    }

    public void infect()
    {
        StartCoroutine(GrowTentacles());
    }

    private IEnumerator GrowTentacles()
    {
        int its = 100;
        float alphaInc = (1 - startColour.a) / its;
        for (int i = 0; i < its; i++)
        {
            yield return new WaitForSeconds(0.15f);
            transform.localScale += Vector3.one * 0.0035f;
            foreach (var sprite in sprites)
            {
                Color next = sprite.color;
                next.a += alphaInc;
                sprite.color = next;
            }        
        }
    }
}
