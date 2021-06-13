using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine.AI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    public HashSet<Controller> humans;
    public HashSet<Controller> zombies;
    public HashSet<Controller> infected; // a subset of the humans set, for all humans which are infected
    public GameObject saucePrefab;

    public AnimatorController zombieController;

    private void Awake()
    {
        instance = this;
        humans = new HashSet<Controller>();
        zombies = new HashSet<Controller>();
        infected = new HashSet<Controller>();
    }

    public static void registerHuman(Controller human)
    {
        instance.humans.Add(human);
    }
    
    public static void registerZombie(Controller zombie)
    {
        instance.zombies.Add(zombie);
    }

    public static void bodySnatch(Controller human, Player currentPlayer)
    {
        // Debug.Log("body snatching " + human);
        human.character.infect(); // star ts timer to become a zombie
        GameObject playerObject = currentPlayer.gameObject;
        Sauce sauce = playerObject.GetComponent<Sauce>();
        if (sauce != null) // wasn't in a body yet
        {
            Destroy(playerObject);
        }
        else
        {
            humanify(currentPlayer);
        }

        instance.humans.Remove(human);
        GameObject humanObject = human.gameObject;
        Destroy(humanObject.GetComponent<Human>());
        human.glowEffect.gameObject.SetActive(true);
        Player player = humanObject.AddComponent<Player>();
        Player.instance = player;
        humanObject.name = "Player";
        humanObject.layer = LayerMask.NameToLayer("player");
        human.glowTimeLeft = 0;
        humanObject.GetComponent<NavMeshAgent>().enabled = false;
        humanObject.GetComponent<Animator>().enabled = false;
        
        UIManager.setCurrentHost(player.character);
    }


    // ReSharper disable Unity.PerformanceAnalysis
    public static void zombify(Controller controller)
    {
        GameObject go = controller.gameObject;
        if (controller is Human)
        {//is human, so player has left the character already
            instance.humans.Remove(controller);
            instance.infected.Remove(controller);
        } else if (controller is Player){ }
        else
        {
            throw new Exception("unreckognised type");
        }
        
        controller.glowEffect.gameObject.SetActive(true);
        Destroy(controller);
        Zombie zom = go.AddComponent<Zombie>();
        instance.zombies.Add(zom);
        go.name = "Zombie";
        go.layer = LayerMask.NameToLayer("zombie");
        zom.GetComponent<NavMeshAgent>().enabled = true;
        zom.GetComponent<Animator>().enabled = true;
        // Debug.Log("zombification complete");
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public static void humanify(Controller controller) // turns a host back into a human temporarily
    {
        GameObject hostObj = controller.gameObject;
        controller.glowEffect.gameObject.SetActive(true);
        Destroy(hostObj.GetComponent<Player>());
        Human human = hostObj.AddComponent<Human>();
        human.enabled = true;
        hostObj.name = "Infected Human";
        hostObj.layer = LayerMask.NameToLayer("human");
        instance.infected.Add(human);
        instance.humans.Add(human);
        hostObj.GetComponent<NavMeshAgent>().enabled = true;
        hostObj.GetComponent<Animator>().enabled = true;
    }

    private HashSet<Controller> getVisibleCharacters(Controller looker, HashSet<Controller> controllers)
    {
        HashSet<Controller> visible = new HashSet<Controller>();
        foreach (var controller in controllers)
        {
            if (looker is Player)
            {
                if (looker.checkVisisble(controller.gameObject, visionDistance: Player.infectionDistance, visionAngle: Player.infectionAngle))
                {
                    visible.Add(controller);
                }
            }
            else if (looker is Zombie)
            {
                if (looker.checkVisisble(controller.gameObject, visionDistance: Zombie.visionDistance, visionAngle: Zombie.visionAngle))
                {
                    visible.Add(controller);
                }
            }
            else
            {
                if (looker.checkVisisble(controller.gameObject))
                {
                    visible.Add(controller);
                }
            }

        }

        return visible;
    }

    public static HashSet<Controller> getVisibleHumans(Controller looker)
    {
        return instance.getVisibleCharacters(looker, instance.humans);
    }
    
    public static HashSet<Controller> getVisibleZombies(Controller looker)
    {
        return instance.getVisibleCharacters(looker, instance.zombies);
    }

    public static HashSet<Controller> getVisibleInfected(Controller looker)
    {
        return instance.getVisibleCharacters(looker, instance.infected);
    }

    public static HashSet<Controller> getVisibleHorde(Controller looker)
    {
        HashSet<Controller> zombies = instance.getVisibleCharacters(looker, instance.zombies);
        // Debug.Log("num visible zombies: " + zombies.Count + " num zombies: " + instance.zombies.Count);
        HashSet<Controller> infected = instance.getVisibleCharacters(looker, instance.infected);
        // Debug.Log("num visible infected: " + infected.Count + " num infected: " + instance.infected.Count);
        zombies.UnionWith(infected);
        HashSet<Controller> horde = zombies;
        if (looker.checkVisisble(Player.instance.gameObject))
        {
            horde.Add(Player.instance.GetComponent<Controller>());
        }
        // Debug.Log("num visible in horde: " + horde.Count);
        
        return horde;
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
}
