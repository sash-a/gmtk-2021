using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float moveSpeed = 10;
    public float visionDistance = 10;
    public float visionAngle = 90;
    public float infectionTime = 10; // how much time from infection until zombification
    private float timeOfInfection;  // -1 if not infected

    public abstract void die();

    public void infect()
    {
        timeOfInfection = Time.time;
        StartCoroutine(WaitAndZombify());
    }

    private IEnumerator WaitAndZombify()
    {
        Debug.Log("waiting to zombify");
        yield return new WaitForSeconds(infectionTime);
        zombify();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void zombify()
    {
        Debug.Log("zombifying " + this);
        Player player = GetComponent<Player>();
        if (player != null) // player must be ejected
        {
            eject(player);
            CharacterManager.zombify(player);
        }
        else
        { // player has already left
            Human human = GetComponent<Human>();
            CharacterManager.zombify(human);
        }

        
    }
    
    public void eject(Player player) // method should be called when the player leaps out of the character
    {
        GameObject newSauce = Instantiate(CharacterManager.instance.saucePrefab);
    }
}
