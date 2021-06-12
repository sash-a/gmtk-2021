using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Ai
{
    private void Start()
    {
        CharacterManager.registerHuman(this);
    }

    private void FixedUpdate()
    {
        print("visible:" + checkVisisble(Player.instance.gameObject));

        if (Input.GetKeyUp(KeyCode.Z))
        {
            character.infect();
        }
    }
}
