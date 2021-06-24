using System.Collections.Generic;
using UnityEngine;

public class Human : Ai
{
    private void Start()
    {
        CharacterManager.registerHuman(this);
        base.Start();   
    }

    private void FixedUpdate()
    {
        if(transform.GetComponent<Ai>().agent.velocity != Vector3.zero)
        {
            AudioManager.instance.Play("footstep_1");
        }

        base.FixedUpdate();
    }

    public override bool checkVisisble(GameObject go, float visionAngle=-1, float visionDistance=-1, List<string> layers = null)
    {
        layers = new List<string>();
        layers.AddRange(new []{"player", "infected", "wall", "zombie"});
        return base.checkVisisble(go, visionAngle, visionDistance, layers);
    }
}
