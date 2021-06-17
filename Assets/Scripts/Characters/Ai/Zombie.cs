using System;
using UnityEngine;


public class Zombie : Ai
{
    public static float visionDistance = 7;
    public static float visionAngle = 360;
    
    private void Start()
    {
        CharacterManager.registerZombie(this);
        base.Start();
    }
}
