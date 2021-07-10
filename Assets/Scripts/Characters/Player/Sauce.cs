using System;
using System.Collections;
using System.Collections.Generic;
using State_Machine;
using UnityEngine;

public class Sauce : Character
{
    public Animator sauceAnimator;

    private void Awake()
    {
        UIManager.setCurrentHost(this);
    }

    private void OnCollisionEnter2D(Collision2D other) // checks if the slime has slid into a character
    {
        Controller cntrl = other.gameObject.GetComponent<Controller>();
        if (cntrl is Human)
        {
            // can infect human
            if (Player.instance.exitedHost == cntrl.character && Player.instance.remainingSlideTime >0)
            {  //just slid out this host
                return;
            }
            
            if (Player.instance.canInfectHost(cntrl))
            {
                TransitionManager.bodySnatch((Human) cntrl, GetComponent<Player>());
            }
        }
    }
}