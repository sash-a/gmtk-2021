using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    public HashSet<Controller> humans;
    public HashSet<Controller> zombies;

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
        Destroy(playerObject);
        instance.humans.Remove(human);
        GameObject humanObject = human.gameObject;
        Destroy(humanObject.GetComponent<Human>());
        Player player = humanObject.AddComponent<Player>();
        Player.instance = player;
        CameraFollow.instance.target = humanObject.transform;
        humanObject.name = "Player";
        humanObject.layer = LayerMask.NameToLayer("player");
        human.glowEffect.gameObject.SetActive(false);
        human.glowTimeLeft = 0;
    }


    // ReSharper disable Unity.PerformanceAnalysis
    public static void zombify(Human human)
    {
        GameObject go = human.gameObject;
        instance.humans.Remove(human);
        Destroy(human);
        Zombie zom = go.AddComponent<Zombie>();
        instance.zombies.Add(zom);
        Debug.Log("zombification complete");
    }

    private HashSet<Controller> getVisibleCharacters(Controller looker, HashSet<Controller> controllers)
    {
        HashSet<Controller> visible = new HashSet<Controller>();
        foreach (var controller in controllers)
        {
            if (looker.checkVisisble(controller.gameObject))
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
    
    public static HashSet<Controller> getVisibleZombies(Controller looker)
    {
        return instance.getVisibleCharacters(looker, instance.zombies);
    }
    
}
