using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeManager : MonoBehaviour
{
    // melee manager is for Ai <-> Ai melee only
    // keeps track of which ai has hit me, and which ai i have hit
    // overrides the dying logic us usual melee fighting to make Zombie vs Human melee fights consistent and probabilistic
    
    //One a hit has occured, a timer begins, after which, if the hit was reciprocal, the fight is decided probabilistically 
    // if the timer passes and the hit is one way only, then the hitter kills the receiver 

    public static float delayTime = 0.3f;  // how long to wait for a reciprocal hit
    public static float zombieWinChance = 0.5f;

    private Melee _me;
    HashSet<Melee> hasHitMe;

    private void Awake()
    {
        hasHitMe = new HashSet<Melee>();
    }

    public Melee me
    {
        get
        {
            if (_me == null)
            {
                _me = GetComponent<Melee>();
            }

            return _me;
        }
    }

    public void hitAi(Melee receiver)
    {
        if (receiver.aiMeleeManager.hasHitMe.Contains(me))
        { // this ai has already hit the receiver
            return;
        }

        receiver.aiMeleeManager.getHitByAi(me);
        StartCoroutine(resolveMyHit(receiver));  // delay, then check if hit is reciprocal
    }

    private IEnumerator resolveMyHit(Melee receiver) // I have hit this ai
    {
        yield return new WaitForSecondsRealtime(delayTime); // sufficient time passed for reciprocation
        bool iWin = false;
        if (hasHitMe.Contains(receiver))
        { // this hit was reciprocal
            //Debug.Log("symetrical hit");
            float myWinChance = me.myController is Zombie ? zombieWinChance : (1 - zombieWinChance);
            if (Random.Range(0f, 1f) < myWinChance)
            {
                iWin = true;
            }
        }   
        else
        { // one sided hit
            //Debug.Log("one sided hit");
            iWin = true;
        }

        if (iWin)
        {
            try
            {
                receiver.die();
            }
            catch (MissingReferenceException e)
            { // has already died
            } 
        }
    }

    private void getHitByAi(Melee hitter)
    {
        hasHitMe.Add(hitter);
    }
}
