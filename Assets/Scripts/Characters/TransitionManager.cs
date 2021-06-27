using System;
using UnityEngine;
using UnityEngine.AI;

public class TransitionManager : MonoBehaviour
{
    public static void bodySnatch(Controller human, Player currentPlayer)
    {
        AudioManager.instance.PlayRandom(new[] {"gargle_1", "gargle_2"});

        human.character.infect(); // starts timer to become a zombie
        Sauce sauce = currentPlayer.GetComponent<Sauce>();
        if (sauce != null) // wasn't in a body yet
        {
            Destroy(currentPlayer.gameObject);
        }
        else
        {
            humanify(currentPlayer);
        }

        // Get game object ref and remove human AI script from it
        CharacterManager.instance.RemoveHuman(human);
        GameObject humanObject = human.gameObject;
        Destroy(humanObject.GetComponent<Human>());
        humanObject.GetComponent<NavMeshAgent>().enabled = false;
        humanObject.GetComponent<Animator>().enabled = false;

        // Setting up game object for player control
        Player player = humanObject.AddComponent<Player>();
        Player.instance = player;
        humanObject.name = "Player";
        humanObject.layer = LayerMask.NameToLayer("player");

        human.rb.isKinematic = false;
        human.character.glowTimeLeft = 0;
        human.character.glowEffect.gameObject.SetActive(false);
        human.character.tentacles.infect();

        UIManager.setCurrentHost(player.character);
    }

    /**
     * Adds controller to zombie horde and turns human ai into zombie
     */
    public static void zombify(Controller controller)
    {
        AudioManager.instance.PlayRandom(new[] {"groan_1", "groan_2"});
        GameObject go = controller.gameObject;
        controller.character.glowEffect.gameObject.SetActive(false);

        switch (controller)
        {
            case Human _: //  is human, so player has left the character already
                CharacterManager.instance.RemoveHuman(controller);
                CharacterManager.instance.RemoveInfected(controller);
                break;
            case Player _:
                break;
            default:
                throw new Exception("unrecognised type");
        }

        Destroy(controller);

        Zombie zom = go.AddComponent<Zombie>();
        CharacterManager.registerZombie(zom);
        go.name = "Zombie";
        go.layer = LayerMask.NameToLayer("zombie");

        if (zom.character is Ranged ranged)
        {
            Destroy(ranged);
            Melee mel = zom.GetComponent<Melee>();
            mel.enabled = true;
            zom.character = mel;
        } // convert the ranged attacker to a melee attacker

        zom.character.tentacles.makeZombie();
        zom.character.rb.isKinematic = true;
        zom.character.SpriteController.torsoAnimator.SetBool("iszombie", true);

        zom.GetComponent<NavMeshAgent>().enabled = true;
        zom.GetComponent<Animator>().enabled = true;
    }

    /**
     * turns a host back into a human temporarily
     */
    public static void humanify(Controller host)
    {
        AudioManager.instance.PlayRandom(new[] {"cough_spit_1", "cough_spit_2"});
        GameObject hostGo = host.gameObject;
        host.character.glowEffect.gameObject.SetActive(false);

        host.character.humanify();
        host.rb.isKinematic = true;

        Destroy(hostGo.GetComponent<Player>());

        Human human = hostGo.AddComponent<Human>();
        human.enabled = true;
        hostGo.name = "Infected Human";

        CharacterManager.registerInfected(human);
        CharacterManager.registerHuman(human);

        hostGo.GetComponent<NavMeshAgent>().enabled = true;
        hostGo.GetComponent<Animator>().enabled = true;
    }
}