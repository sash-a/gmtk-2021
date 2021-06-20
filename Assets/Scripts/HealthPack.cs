using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{

    public SpriteRenderer sprite;

    public float frequency = 2.2f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        sprite.color = new Color(Mathf.Sin(Time.time * frequency) * 0.5f + 0.5f, Mathf.Sin(Time.time * frequency)* -0.5f + 0.5f, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character ch = other.GetComponent<Controller>().character;
        if (ch is Sauce)
        {
            return; // sauce cannot pick up health
        }
        ch.timeOfInfection = Time.time;
        Destroy(gameObject);
    }
}
