using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectionBar : MonoBehaviour
{
    public Slider slider;
    public Character infectedCharacter;
    private void Update()
    {
        if (infectedCharacter is Sauce)
        {
            slider.value = 1;
            return; 
        }

        if (Math.Abs(infectedCharacter.timeOfInfection - (-1)) < 0.0001f)
        {
            throw new Exception("uninfected character cannot  have infection bar");
        }

        slider.value = infectedCharacter.getInfectionFrac();
    }
}
