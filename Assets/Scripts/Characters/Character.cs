using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float moveSpeed;
    public float visionDistance;
    public float visionAngle;
    public float infectionTime; // how much time from infection until zombification
    private float timeOfInfection;  // -1 if not infected


    public abstract void die();
    
    public void infect()
    {
        timeOfInfection = Time.time;
        StartCoroutine(WaitAndZombify());
    }

    private IEnumerator WaitAndZombify()
    {
        yield return new WaitForSeconds(infectionTime);
        zombify();
    }

    private void zombify()
    {
        Player player = GetComponent<Player>();
        if (player != null)
        {
            throw new Exception("player is in zombified character");
        }

        Human human = GetComponent<Human>();
        CharacterManager.zombify(human);
    }
}
