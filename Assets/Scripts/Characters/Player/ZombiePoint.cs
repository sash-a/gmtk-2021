using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using State_Machine;
using Unity.VisualScripting;
using UnityEngine;

public class ZombiePoint : MonoBehaviour
{
    public HashSet<Controller> containedZombies;

    private void Awake()
    {
        containedZombies = new HashSet<Controller>();
    }

    public void addZombies(HashSet<Controller> zombies)
    {
        containedZombies.AddRange(zombies);
        foreach (var zombie in zombies)
        {
            if (zombie is Ai zom)
            {
                var anim = zom.GetComponent<Animator>();
                Patrol patrol = anim.GetBehaviour<Patrol>();
                if (patrol != null)
                {
                    patrol.updateZombieWaypoints();
                }
            }
        }
    }

    public void removeZombie(Controller zom)
    {
        containedZombies.Remove(zom);
        if (containedZombies.Count == 0)
        {
            Destroy(gameObject);
        }
    }
}
