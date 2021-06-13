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
    public float timeOfInfection = -1;  // -1 if not infected

    public void die()
    {
        Player player = GetComponent<Player>();
        if (player != null) {
            throw new Exception("player died");
        }
        Zombie zom = GetComponent<Zombie>();
        if (zom != null)
        {
            CharacterManager.instance.zombies.Remove(zom);
        }
        else {
            Human human = GetComponent<Human>();
            if (timeOfInfection != -1) {
                CharacterManager.instance.infected.Remove(human);
            }
            CharacterManager.instance.humans.Remove(human);
        }
        
        Destroy(gameObject);
    }
    public CharacterSpriteController SpriteController;

    public void infect()
    {
        if (Math.Abs(timeOfInfection - (-1)) < 0.00001f)
        {
            timeOfInfection = Time.time;
        }

        StartCoroutine(WaitAndZombify());
    }

    private IEnumerator WaitAndZombify()
    {
        // Debug.Log("waiting to zombify");
        yield return new WaitForSeconds(infectionTime);
        zombify();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void zombify()
    {
        // Debug.Log("zombifying " + this);
        Player player = GetComponent<Player>();
        if (player != null) // player must be ejected
        {
            eject();
            CharacterManager.zombify(player);
        }
        else
        { // player has already left
            Human human = GetComponent<Human>();
            if (human != null)
            {
                CharacterManager.zombify(human);
            } // else is already a zombie (don't really understand this behaviour, but it seems to work fine)
        }

        
    }
    
    public void eject() // method should be called when the player leaps out of the character
    {
        Vector3 ejectPos = transform.position + transform.right * Player.ejectDistance;
        GameObject newSauce = Instantiate(CharacterManager.instance.saucePrefab, ejectPos, transform.rotation);
    }
}
