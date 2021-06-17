using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Attacker
{
    public MeleeHitBox hitBox;
    private Controller myController;
    
    public override void Attack(Vector3 dir = new Vector3(), bool isPlayer = false)
    {

        AudioManager.instance.PlayRandom(new string[] { "melee_1", "melee_2" });

        if (myController == null)
        {
            myController = GetComponent<Controller>();
        }
        
        SpriteController.torsoAnimator.SetBool("Attacking", true);
        
        StartCoroutine(WaitAndHit(isPlayer));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator WaitAndHit(bool isPlayer)
    {
        List<Controller> hits = new List<Controller>();


        if (isPlayer)
        {
            yield return new WaitForEndOfFrame();
        }
        if (GetComponent<Zombie>() != null)
        {
            yield return new WaitForSecondsRealtime(0.32f);
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.22f);
        }
        
        foreach (var touched in hitBox.touchedCharacters)
        {
            if (myController is Zombie || myController is Player)
            {
                if (touched is Human)
                {
                    hits.Add(touched);
                }
            }
            else{ // my controller is human
                if (touched is Zombie || touched is Player)
                {
                    hits.Add(touched);
                }
            }
        }
        for (int i = 0; i < hits.Count; i++)
        {
            hits[i].character.die();
        }
        
        SpriteController.torsoAnimator.SetBool("Attacking", false);
    }

    private void Update() // don't remove. needed to make script disablable
    {
    }
}
