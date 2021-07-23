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

    public float levelStartTime;
    
    // public AnimatorController zombieController;

    private void Awake()
    {
        instance = this;
        humans = new HashSet<Controller>();
        zombies = new HashSet<Controller>();
        infected = new HashSet<Controller>();
        levelStartTime = Time.time;
    }

    public int getNumberOfHumans()
    {
        return humans.Count;
    }

    public int getNumberUninfectedHumans()
    {
        return humans.Count - infected.Count;
    }
    
    public int getNumberInfected()
    {
        return infected.Count + zombies.Count;
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
    
    private HashSet<Controller> getVisibleCharacters(Controller looker, HashSet<Controller> controllers, bool recoveryMode=false)
    {
        if (looker == null)
        {
            throw new Exception("looker is null");
        }

        HashSet<Controller> visible = new HashSet<Controller>();
        foreach (var controller in controllers)
        {
            if (controller == null)  // recover and try again
            {
                Debug.LogWarning("null controllers in character manager. Recovering");
                if (recoveryMode)
                {
                    throw new Exception("null controllers in character manager. Recovery failed");
                }
                refreshSets();
                return getVisibleCharacters(looker, controllers, recoveryMode:true);
            }
            if (looker.checkVisible(controller.gameObject))
            {
                visible.Add(controller);
            }
        }

        return visible;
    }

    private void refreshSets()
    {   // there are some null elements in our sets. refresh them
        humans = getRefreshedSet(humans);
        zombies = getRefreshedSet(zombies); 
        infected = getRefreshedSet(infected);
    }

    private HashSet<Controller> getRefreshedSet(HashSet<Controller> controllers)
    {
        HashSet<Controller> _controllers = new HashSet<Controller>();
        foreach (var controller in controllers)
        {
            if (controller == null)
            {
                continue;
            }
            _controllers.Add(controller);
        }

        return _controllers;
    }

    public static HashSet<Controller> getVisibleHumans(Controller looker)
    {
        return instance.getVisibleCharacters(looker, instance.humans);
    }
    
    public static HashSet<Controller> getVisibleInfected(Controller looker)
    {
        return instance.getVisibleCharacters(looker, instance.infected);
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
        HashSet<Controller> visible_infected = instance.getVisibleCharacters(looker, instance.infected);
        foreach (var inf in visible_infected)
        {
            if (((Human) looker).sussPeople.Contains(inf.character))
            { // this infected person was sussed
                visible_zombies.Add(inf);
            }
        }
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

            if (Time.time - Player.instance.character.lastShotTime < Character.shotSoundTime &&
                Player.instance.character.lastShotTime != -1)
            {
                //human has shot very recently. we will add them to nearby humans suss list
                float distance = Vector2.Distance(Player.instance.transform.position, looker.transform.position);
                if (distance < Character.shotSoundDistance)
                {
                    // looker is close enough to hear the gun shot
                    if (looker.checkVisible(Player.instance.gameObject, visionAngle:360))
                    {  // clean line of sight to the shooter, they are added to the suss list
                        ((Human) looker).sussPeople.Add(Player.instance.character);
                    }
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
            HashSet<Controller> humans = getVisibleHumans(looker);
            return humans;
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