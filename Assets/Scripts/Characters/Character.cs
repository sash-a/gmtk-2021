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
    [NonSerialized] public float lastAttacked;


    public void Awake()
    {
        waypoints = GetComponentInChildren<Waypoints>();
        waypoints.compute();
        rb = GetComponent<Rigidbody2D>();
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
        if (Math.Abs(timeOfInfection - (-1)) < 0.00001f)
        {
            timeOfInfection = Time.time;
        }

        StartCoroutine(WaitAndZombify());
    }

    private IEnumerator WaitAndZombify()
    {
        //Debug.Log("waiting to zombify");
        yield return new WaitForSeconds(infectionTime);
        while (getInfectionFrac() < 0.99f && getInfectionFrac() != -1)
        {
            //Debug.Log("FRAC:" + getInfectionFrac());
            yield return new WaitForSeconds(infectionTime/3f);
        }
        zombify();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void zombify()
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

        var anim = GetComponent<Animator>();
        // anim.Play(Zombiefying);
        anim.SetBool(AnimatorFields.Patrolling, true);
        anim.SetBool(AnimatorFields.Zombiefying, true);
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
        lastAttacked = Time.time;
    }
}
