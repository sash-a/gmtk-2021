using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPubble : MonoBehaviour
{
    private float spawnTime;
    public static float lifetime = 3;
    public static float fadeFac = 0.99f;
    public SpriteRenderer renderer;
    
    private void Start()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - spawnTime > lifetime)
        {
            Destroy(gameObject);
        }

        Color newColour = renderer.color;
        newColour.a *= fadeFac;
        renderer.color = newColour;     
    }
}
