using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class Character : MonoBehaviour
{
    public float moveSpeed = 10;
    public float visionDistance = 10;
    public float visionAngle = 90;
    public float infectionTime = 6; // how much time from infection until zombification
    [NonSerialized] public float timeOfInfection = -1;  // -1 if not infected

    public List<Transform> waypointTransforms;
    [NonSerialized] public List<Vector3> waypoints;

    public GameObject redBloodPuddle;
    public GameObject greenBloodPuddle;
    public GameObject dieEffect;
    public GameObject greenDieEffect;
    
    public Sprite face0;
    public Sprite face1;
    public Sprite face2;

    private bool playerDead = false;

    private void Awake()
    {
        waypoints = new List<Vector3>();
        foreach (var waypoint in waypointTransforms)
        {
            waypoints.Add(waypoint.position);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void die()
    {
        Player player = GetComponent<Player>();
        if (player != null && !playerDead)
        {
            Instantiate(greenBloodPuddle,transform.position, transform.rotation);
            Instantiate(greenDieEffect,transform.position, transform.rotation);
            Debug.Log("Playing!");
            StartCoroutine(WaitAndRespawn());
            return;
        }
        Zombie zom = GetComponent<Zombie>();
        if (zom != null)
        {
            Instantiate(greenBloodPuddle,transform.position, transform.rotation);
            Instantiate(greenDieEffect,transform.position, transform.rotation);
            Debug.Log("Playing!");

            CharacterManager.instance.RemoveZombie(zom);
        }
        else {
            Human human = GetComponent<Human>();
            if (timeOfInfection != -1) {
                CharacterManager.instance.RemoveInfected(human);
            }

            Instantiate(redBloodPuddle,transform.position, transform.rotation);
            Instantiate(dieEffect,transform.position, transform.rotation);
            Debug.Log("Playing!");


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
        if (Math.Abs(timeOfInfection - (-1)) < 0.00001f)
        {
            timeOfInfection = Time.time;
        }

        StartCoroutine(WaitAndZombify());
    }

    private IEnumerator WaitAndZombify()
    {
        // Debug.Log("waiting to zombify");
        yield return new WaitForSeconds(infectionTime);
        zombify();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void zombify()
    {
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

        
    }
    
    public void eject() // method should be called when the player leaps out of the character
    {
        Vector3 ejectPos = transform.position + transform.right * Player.ejectDistance;
        GameObject newSauce = Instantiate(CharacterManager.instance.saucePrefab, ejectPos, transform.rotation);
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
