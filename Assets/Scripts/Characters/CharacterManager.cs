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
    public HashSet<Controller> zombies;
    public HashSet<Controller> infected; // a subset of the humans set, for all humans which are infected

    [NonSerialized] public float levelStartTime;
    
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
                    Debug.LogError("null controllers in character manager. Recovery failed");
                    continue;
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
        if (VisibilityManager.isPlayerEffectivelyVisible(looker))
        {
            visible_zombies.Add(Player.instance);
        }        

        return visible_zombies;
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