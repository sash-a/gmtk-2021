using System;
using System.Collections.Generic;
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
        HashSet<Controller> visible_zombies = instance.getVisibleCharacters(looker, instance.zombies);
        // Debug.Log("num visible zombies: " + zombies.Count + " num zombies: " + instance.zombies.Count);
        bool playerVisible = false;
        if (Player.instance.character is Sauce)
        {
            if (Player.instance.remainingSlideTime <= 0)
            {
                if (looker.checkVisible(Player.instance.gameObject)) 
                {
                    visible_zombies.Add(Player.instance);
                }
            }
        }
        else
        { // is in human. is visible if has shot recently
            //Debug.Log("player is human");
            if (Time.time - Player.instance.character.lastShot < Character.shotSoundTime && Player.instance.character.lastShot != -1)
            {//human has shot recently
                //Debug.Log("player has shot. shot time " + Player.instance.character.lastShot + " time: " + Time.time);
                float distance = Vector2.Distance(Player.instance.transform.position, looker.transform.position);
                if (distance < Character.shotSoundDistance)
                {// looker is close enough to hear the gun shot
                    visible_zombies.Add(Player.instance);
                }
            }
        }

        return visible_zombies;
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