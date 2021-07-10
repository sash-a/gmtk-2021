using System;
using System.Collections.Generic;
using UnityEngine;

public class Human : Ai
{
    [NonSerialized] public HashSet<Character> sussPeople;
    private void Start()
    {
        sussPeople = new HashSet<Character>();
        CharacterManager.registerHuman(this);
    }

    private void FixedUpdate()
    {
        if(transform.GetComponent<Ai>().agent.velocity != Vector3.zero)
        {
            AudioManager.instance.Play("footstep_1");
        }

        base.FixedUpdate();
    }

    public override bool checkVisible(GameObject go, float visionAngle=-1, float visionDistance=-1, List<string> layers = null)
    {
        layers = new List<string>();
        layers.AddRange(new []{"player", "infected", "wall", "zombie"});
        return base.checkVisible(go, visionAngle, visionDistance, layers);
    }
}
