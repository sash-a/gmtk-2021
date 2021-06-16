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
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(0.15f);
            transform.localScale += Vector3.one * 0.01f;
        }
    }
}
