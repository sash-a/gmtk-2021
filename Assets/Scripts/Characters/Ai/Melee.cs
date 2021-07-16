using System;
using System.Collections;
using System.Collections.Generic;
using State_Machine;
using UnityEngine;

public class Melee : Attacker
{
    public MeleeHitBox hitBox;
    public Controller myController;
    public MeleeManager aiMeleeManager;
    
    public new void Awake()
    {
        base.Awake();
        aiMeleeManager = GetComponent<MeleeManager>();
    }
    
    public override void Attack(Vector3 dir = new Vector3(), bool isPlayer = false)
    {

        AudioManager.instance.PlayRandom(new string[] { "melee_1", "melee_2" });

        if (myController == null)
        {
            myController = GetComponent<Controller>();
        }
        base.Attack();

        SpriteController.torsoAnimator.SetBool(AnimatorFields.Attacking, true);
        
        StartCoroutine(WaitAndHit(isPlayer));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator WaitAndHit(bool isPlayer)
    {
        List<Controller> instantHits = new List<Controller>();


        if (isPlayer )
        {
            yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.22f);
        }

        foreach (var touched in hitBox.touchedCharacters)
        {
            //Debug.Log(myController + " touching " + touched);
            if (myController is Zombie)
            {
                
                if (touched is Human)
                {
                    if (touched.character is Melee)
                    {  // melee X melee fight, should be handled by the melee manager
                        ((Melee)myController.character).aiMeleeManager.hitAi((Melee)touched.character);
                    }
                    else
                    {
                        instantHits.Add(touched);
                    }
                }
            }

            if (myController is Player)
            {
                if (touched is Human)
                {
                    instantHits.Add(touched);
                }
            }

            if (myController is Human)
            {
                if (touched is Zombie)
                {
                    if (touched.character is Melee)
                    {  // melee X melee fight, should be handled by the melee manager
                        ((Melee)myController.character).aiMeleeManager.hitAi((Melee)touched.character);
                    }
                    else
                    {
                        instantHits.Add(touched);
                    }
                }

                if (touched is Player)
                {
                    instantHits.Add(touched);
                }

                if (touched is Human human)
                {
                    if (human.character.isInfected())
                    {
                        instantHits.Add(touched);
                    }
                }
            }
        }
        for (int i = 0; i < instantHits.Count; i++)
        {
            try
            {
                instantHits[i].character.die();
            }
            catch (Exception e)
            {// is killed during wait for delay, can throw error. is edge case
                break;
            }
        }
        
        SpriteController.torsoAnimator.SetBool(AnimatorFields.Attacking, false);
    }

    private void Update() // don't remove. needed to make script disablable
    {
    }
}
