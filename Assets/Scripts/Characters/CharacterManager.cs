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
        AudioManager.instance.PlayRandom(new string[] {"gargle_1", "gargle_2"});
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

        instance.RemoveHuman(human);
        GameObject humanObject = human.gameObject;
        Destroy(humanObject.GetComponent<Human>());
        human.glowEffect.gameObject.SetActive(true);
        Player player = humanObject.AddComponent<Player>();
        Player.instance = player;
        humanObject.name = "Player";
        humanObject.layer = LayerMask.NameToLayer("player");
        human.glowTimeLeft = 0;
        human.character.tentacles.infect();
        humanObject.GetComponent<NavMeshAgent>().enabled = false;
        humanObject.GetComponent<Animator>().enabled = false;
        
        UIManager.setCurrentHost(player.character);
    }


    // ReSharper disable Unity.PerformanceAnalysis
    public static void zombify(Controller controller)
    {
        AudioManager.instance.PlayRandom(new string[] { "groan_1", "groan_2" });
        GameObject go = controller.gameObject;
        if (controller is Human)
        {//is human, so player has left the character already
            instance.RemoveHuman(controller);
            instance.RemoveInfected(controller);
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
        zom.character.tentacles.makeZombie();
        zom.character.SpriteController.torsoAnimator.SetBool("iszombie", true);

        if (zom.character is Ranged)
        {
            Ranged ran = (Ranged)zom.character;
            
            Melee mel = zom.AddComponent<Melee>();
            GameObject hitbox = Instantiate(instance.meleeBoxPrefab, mel.transform);
            mel.hitBox = hitbox.GetComponent<MeleeHitBox>();
            mel.face0 = ran.face0;
            mel.face1 = ran.face1;
            mel.face2 = ran.face2;

            mel.dieEffect = ran.dieEffect;
            mel.greenBloodPuddle = ran.greenBloodPuddle;
            mel.greenDieEffect = ran.greenDieEffect;

            mel.redBloodPuddle = ran.redBloodPuddle;
            mel.SpriteController = ran.SpriteController;
            mel.attackRange = 1;
            mel.waypointTransforms = ran.waypointTransforms;
            zom.character = mel;
            Destroy(ran);
        }  // convert the ranged attacker to a melee attacker
        // Debug.Log("zombification complete");
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public static void humanify(Controller controller) // turns a host back into a human temporarily
    {

        AudioManager.instance.PlayRandom(new string[] { "cough_spit_1", "cough_spit_2" });
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
        bool playerVisible = false;
        if (looker.checkVisisble(Player.instance.gameObject))
        {
            infected.Add(Player.instance);
            playerVisible = true;
        }
        
        foreach (var inf in infected)
        {
            float frac = inf.character.getInfectionFrac();
            if(frac < 0.33f){ continue; } // full incognito

            if (frac < 0.66f) // partial suss/ partial incognito
            {
                if (looker.checkVisisble(inf.gameObject, looker.character.visionAngle / 2f,
                    looker.character.visionDistance / 2f))
                { // is very in view
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
        if (humans.Count <= 0)
        {
            Debug.Log("WIN Level!");
        }
    }
}
