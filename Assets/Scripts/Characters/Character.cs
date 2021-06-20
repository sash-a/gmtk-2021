using System;
using System.Collections;
using System.Collections.Generic;
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

    public Tentacles tentacles;

    public void Awake()
    {
        waypoints = GetComponentInChildren<Waypoints>();
        waypoints.compute();
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
    private static readonly int Zombiefying = Animator.StringToHash("zombiefying");
    private static readonly int IsPatroling = Animator.StringToHash("isPatroling");

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
        // Debug.Log("zombifying " + this);
        Player player = GetComponent<Player>();
        if (player != null) // player must be ejected
        {
            eject();
            CharacterManager.zombify(player);
        }
        else
        { // player has already left
            Human human = GetComponent<Human>();
            if (human != null)
            {
                CharacterManager.zombify(human);
            } // else is already a zombie (don't really understand this behaviour, but it seems to work fine)
        }

        var anim = GetComponent<Animator>();
        // anim.Play(Zombiefying);
        anim.SetBool(IsPatroling, true);
        anim.SetBool(Zombiefying, true);
    }
    
    public void eject() // method should be called when the player leaps out of the character
    {
        Vector3 ejectPos = transform.position + transform.right * Player.ejectForce;
        GameObject newSauce = Instantiate(CharacterManager.instance.saucePrefab, transform.position + transform.right * 0.2f, transform.rotation);
        //newSauce.GetComponent<Rigidbody2D>().MovePosition(transform.position + ejectPos);
        newSauce.GetComponent<Rigidbody2D>().velocity = transform.right * Player.ejectForce;
        newSauce.GetComponent<Player>().remainingSlideTime = 0.2f;
    }   

    public float getInfectionFrac() // -1 if not infected/ already zombie. [0,1] if turning
    {
        if (timeOfInfection == -1 || Time.time - timeOfInfection > infectionTime)
        {
            return -1;
        }
        return (Time.time - timeOfInfection) / infectionTime;
    }
}
