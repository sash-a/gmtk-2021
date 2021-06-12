using System;
using UnityEngine;


public class Zombie : Ai
{
    private void Start()
    {
        CharacterManager.registerZombie(this);
    }
}
