using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Attacker
{
    public MeleeHitBox hitBox;
    private Controller myController;
    
    public override void Attack(Vector3 dir = new Vector3(), bool isPlayer = false)
    {
        if (myController == null)
        {
            myController = GetComponent<Controller>();
        }

        foreach (var touched in hitBox.touchedCharacters)
        {
            if (myController is Zombie || myController is Player)
            {
                if (touched is Human)
                {
                    touched.character.die();
                }
            }
            else{ // my controller is human
                if (touched is Zombie || touched is Player)
                {
                    touched.character.die();
                }
            }
        }
        
        if (myController is Zombie || myController is Player)
        {
            
        }
    }
}
