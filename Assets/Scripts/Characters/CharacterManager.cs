using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    public HashSet<Controller> humans;
    public HashSet<Controller> zombies;
    public GameObject saucePrefab;

    private void Awake()
    {
        instance = this;
        humans = new HashSet<Controller>();
        zombies = new HashSet<Controller>();
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
        Debug.Log("body snatching " + human);
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
    }


    // ReSharper disable Unity.PerformanceAnalysis
    public static void zombify(Controller controller)
    {
        GameObject go = controller.gameObject;
        if (controller is Human)
        {//is human
            instance.humans.Remove(controller);
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
        zom.renderer.color = Color.gray;
        Debug.Log("zombification complete");
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

        instance.humans.Add(human);
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
    
}
