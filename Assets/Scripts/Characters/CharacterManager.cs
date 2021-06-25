using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// using UnityEditor.Animations;
// using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine.AI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    private HashSet<Controller> humans;
    private HashSet<Controller> zombies;
    private HashSet<Controller> infected; // a subset of the humans set, for all humans which are infected
    public GameObject saucePrefab;
    public GameObject meleeBoxPrefab;
    
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

    public static HashSet<Controller> getVisibleHorde(Controller looker)
    {
        HashSet<Controller> zombies = instance.getVisibleCharacters(looker, instance.zombies);
        // Debug.Log("num visible zombies: " + zombies.Count + " num zombies: " + instance.zombies.Count);
        HashSet<Controller> infected = instance.getVisibleCharacters(looker, instance.infected);
        bool playerVisible = false;
        if (looker.checkVisible(Player.instance.gameObject) && Player.instance.remainingSlideTime <= 0) // invisible while sliding
        {
            infected.Add(Player.instance);
            playerVisible = true;
        }

        foreach (var inf in infected)
        {
            float frac = inf.character.getInfectionFrac();
            if (frac < 0.33f)
            {
                continue;
            } // full incognito

            if (frac < 0.66f) // partial suss/ partial incognito
            {
                if (looker.checkVisible(inf.gameObject, looker.character.visionAngle / 2f,
                    looker.character.visionDistance / 2f))
                {
                    // is very in view
                    zombies.Add(inf);
                }
            }
            else // full suss
            {
                zombies.Add(inf);
            }
        }

        if (playerVisible)
        {
            if (Player.instance.character is Sauce)
            {
                zombies.Add(Player.instance);
            }
        }

        return zombies;
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