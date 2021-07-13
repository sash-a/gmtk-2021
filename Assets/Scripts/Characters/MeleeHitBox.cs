using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    public HashSet<Controller> touchedCharacters;

    private void Awake()
    {
        touchedCharacters = new HashSet<Controller>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Controller ctrl = other.gameObject.GetComponent<Controller>();
        if (ctrl != null)
        {
            Debug.Log(gameObject + " touched " + ctrl);
            touchedCharacters.Add(ctrl);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Controller ctrl = other.gameObject.GetComponent<Controller>();
        if (ctrl != null)
        {
            Debug.Log(gameObject + " untouched " + ctrl);
            touchedCharacters.Remove(ctrl);
        }    
    }
}
