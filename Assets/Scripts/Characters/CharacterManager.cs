using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// using UnityEditor.Animations;
// using UnityEditor.U2D.Path.GUIFramework;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    private HashSet<Controller> humans;
    private HashSet<Controller> zombies;
    private HashSet<Controller> infected; // a subset of the humans set, for all humans which are infected
    public GameObject saucePrefab;
    
    // public AnimatorController zombieController;

    private void Awake()
    {
        instance = this;
        humans = new HashSet<Controller>();
        zombies = new HashSet<Controller>();
        infected = new HashSet<Controller>();
    }

    public int getNumberOfHumans()
    {
        return humans.Count;
    }

    public int getNumberUninfectedHumans()
    {
        return humans.Count - infected.Count;
    }

    public static void registerHuman(Controller human)
    {
        instance.humans.Add(human);
    }

    public static void registerZombie(Controller zombie)
    {
        instance.zombies.Add(zombie);
    }

    public static void registerInfected(Controller infected)
    {
        instance.infected.Add(infected);
    }

    public HashSet<Controller> getAllAI()
    {
        return new HashSet<Controller>(instance.humans.Union(instance.infected).Union(instance.zombies));
    }
    
    private HashSet<Controller> getVisibleCharacters(Controller looker, HashSet<Controller> controllers)
    {
        if (looker == null)
        {
            throw new Exception("looker is null");
        }

        HashSet<Controller> visible = new HashSet<Controller>();
        foreach (var controller in controllers)
        {
            if (controller == null)
            {
                throw new Exception("deleted controller still in character manager");
            }
            if (looker.checkVisible(controller.gameObject))
            {
                visible.Add(controller);
            }
        }

        return visible;
    }

    public static HashSet<Controller> getVisibleHumans(Controller looker)
    {
        return instance.getVisibleCharacters(looker, instance.humans);
    }

    public static HashSet<Controller> getAllWitnesses(Controller perpetrator=null)
    { // gets all humans who can see the player/perp
        
        HashSet<Controller> witnesses = new HashSet<Controller>();
        foreach (var human in instance.humans)
        {
            if (human.checkVisible(perpetrator.gameObject))
            {
                witnesses.Add(human);
            }
        }

        return witnesses;
    }

    public static HashSet<Controller> getVisibleHorde(Controller looker)
    { //looker must be a human
        HashSet<Controller> visible_zombies = instance.getVisibleCharacters(looker, instance.zombies);
        if (isPlayerEffectivelyVisible(looker))
        {
            visible_zombies.Add(Player.instance);
        }        

        return visible_zombies;
    }

    public static bool isPlayerEffectivelyVisible(Controller looker) // includes logic around when player can be seen/ is incognito
    {
        if (Player.instance.character is Sauce)
        {
            if (Player.instance.remainingSlideTime <= 0) // done sliding
            {
                if (looker.checkVisible(Player.instance.gameObject))
                {
                    return true;
                }
            }
        }
        else
        {
            // is in human
            if (((Human) looker).sussPeople.Contains(Player.instance.character))
            {
                // this human has seen this host do something suss
                if (looker.checkVisible(Player.instance.gameObject)) // is suspicious and in line of sight
                {  // once sussed, always recognisable
                    return true;
                }
            }

            if (Time.time - Player.instance.character.lastShot < Character.shotSoundTime &&
                Player.instance.character.lastShot != -1)
            {
                //human has shot recently
                float distance = Vector2.Distance(Player.instance.transform.position, looker.transform.position);
                if (distance < Character.shotSoundDistance)
                {
                    // looker is close enough to hear the gun shot
                    return true;
                }
            }
        }

        return false;
    }

    public static HashSet<Controller> getVisibleOfInterest(Controller looker)
    {
        if (looker is Zombie)
        {
            return getVisibleHumans(looker);
        }
        else
        {
            return getVisibleHorde(looker);
        }
    }

    public void RemoveZombie(Zombie zom)
    {
        zombies.Remove(zom);
    }

    public void RemoveInfected(Controller human)
    {
        infected.Remove(human);
    }

    public void RemoveHuman(Controller human)
    {
        humans.Remove(human);
        if (human.character.isInfected())
        {
            infected.Remove(human);
        }
        if (humans.Count <= 0)
        {
            Debug.Log("WIN Level!");
        }
    }
}