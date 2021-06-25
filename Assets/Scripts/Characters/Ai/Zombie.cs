using System;
using System.Collections.Generic;
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
    
    public override bool checkVisible(GameObject go, float visionAngle=-1, float visionDistance=-1, List<string> layers = null)
    {
        layers = new List<string>();
        layers.AddRange(new []{"human", "wall"});
        return base.checkVisible(go, visionAngle, visionDistance, layers);
    }
}
