using System;
using System.Collections;
using System.Collections.Generic;
using State_Machine;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class Character : MonoBehaviour
{
    public float moveSpeed = 7;
    public float visionDistance = 10;
    public float visionAngle = 90;
    public float infectionTime = 8; // how much time from infection until zombification
    [NonSerialized] public float timeOfInfection = -1;  // -1 if not infected

    [NonSerialized] public Waypoints waypoints;

    public GameObject redBloodPuddle;
    public GameObject greenBloodPuddle;
    public GameObject dieEffect;
    public GameObject greenDieEffect;
    
    public Sprite face0;
    public Sprite face1;
    public Sprite face2;

    private bool playerDead = false;
    [NonSerialized] public Rigidbody2D rb;
    public Tentacles tentacles;
    [NonSerialized] public float lastShotTime;
    
    public static float shotSoundDistance = 15;
    public static float shotSoundTime = 0.1f;

    [NonSerialized] public float glowTimeLeft = 0;
    public static float glowTime = 0.25f;
    public CharacterGlowEffect glowEffect;


    public void Awake()
    {
        glowEffect = GetComponentInChildren<CharacterGlowEffect>();
        waypoints = GetComponentInChildren<Waypoints>();
        waypoints.compute();
        rb = GetComponent<Rigidbody2D>();
        lastShotTime = -1;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void die()
    {
        Player player = GetComponent<Player>();
        if (player != null && !playerDead)
        {
            Instantiate(greenBloodPuddle,transform.position, transform.rotation);
            Instantiate(greenDieEffect,transform.position, transform.rotation);
            //Debug.Log("Playing!");
            StartCoroutine(WaitAndRespawn());
            return;
        }
        Zombie zom = GetComponent<Zombie>();
        HazmatGuy hazmatGuy = GetComponent<HazmatGuy>();
        if (hazmatGuy != null)
        {
            if (isInfected() || zom != null)
            {
                hazmatGuy.explode();
            }
        }
        if (zom != null)
        {
            Instantiate(greenBloodPuddle,transform.position, transform.rotation);
            Instantiate(greenDieEffect,transform.position, transform.rotation);
            //Debug.Log("Playing!");

            CharacterManager.instance.RemoveZombie(zom);
        }
        else {
            Human human = GetComponent<Human>();
            if (timeOfInfection != -1) {
                CharacterManager.instance.RemoveInfected(human);
            }

            Instantiate(redBloodPuddle,transform.position, transform.rotation);
            Instantiate(dieEffect,transform.position, transform.rotation);
            //Debug.Log("Playing!");

            CharacterManager.instance.RemoveHuman(human);
        }
        
        Destroy(gameObject);
    }

    private IEnumerator WaitAndRespawn()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public CharacterSpriteController SpriteController;

    public void infect()
    {
        //Debug.Log(this + " infected");
        if (isInfected())
        {
            return;
        }
        timeOfInfection = Time.time;
        tentacles.infect();
        CharacterManager.instance.RemoveHuman(GetComponent<Human>());

        var anim = GetComponent<Animator>();
        anim.SetBool(AnimatorFields.Zombiefying, true);

        StartCoroutine(WaitAndZombify());
    }
    
    public void eject()
    { // called when player leaves te host
        float timeSinceInfection = Time.time - timeOfInfection;
        float remainingInfectionTime = infectionTime - timeSinceInfection;
        remainingInfectionTime /= 2f;
        // set the time of infection retroactively to end infection twice as soon
        timeOfInfection = Time.time + remainingInfectionTime - infectionTime;
    }

    private IEnumerator WaitAndZombify()
    {
        //Debug.Log("waiting to zombify");
        yield return new WaitForSeconds(infectionTime * 0.5f);
        while (getInfectionFrac() < 0.99f && getInfectionFrac() != -1)
        {
            //Debug.Log("FRAC:" + getInfectionFrac());
            yield return new WaitForSeconds(infectionTime/30f);
        }
        zombify();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void zombify()
    {
        // TODO this should go in human and so should the character manager method
        Player player = GetComponent<Player>();
        if (player != null) // player must be ejected
        {
            player.eject();
            TransitionManager.zombify(player);
        }
        else
        { // player has already left
            Human human = GetComponent<Human>();
            if (human != null)
            {
                TransitionManager.zombify(human);
            } // else is already a zombie (don't really understand this behaviour, but it seems to work fine)
        }

    }

    public float getInfectionFrac() // -1 if not infected/ already zombie. [0,1] if turning
    {
        if (timeOfInfection == -1 || Time.time - timeOfInfection > infectionTime)
        {
            return -1;
        }
        return (Time.time - timeOfInfection) / infectionTime;
    }

    public bool isInfected()
    {
        return Math.Abs(getInfectionFrac() - (-1)) > 0.000001f;
    }

    public virtual void Attack(Vector3 dir = new Vector3(), bool isPlayer = false)
    {
        //lastShot = Time.time;
    }
    
    public void glow()
    {
        glowTimeLeft = glowTime;
        if (! glowEffect.gameObject.activeSelf) // not currently glowing
        {
            glowTimeLeft = glowTime;
            glowEffect.gameObject.SetActive(true);
            StartCoroutine(glowForTime());
        }
    }
    
    private IEnumerator glowForTime()
    {
        while (glowTimeLeft > 0)
        {
            yield return new WaitForSeconds(0.05f);
            glowTimeLeft -= 0.05f;
        }
        glowEffect.gameObject.SetActive(false);
    }
}
