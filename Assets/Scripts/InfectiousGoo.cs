using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectiousGoo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Human human = other.GetComponent<Human>();
        if (human != null)
        {
            human.character.infect();
        }
    }
}
