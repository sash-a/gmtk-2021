using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectionBar : MonoBehaviour
{
    public Slider slider;
    [NonSerialized] public Character infectedCharacter;
    [NonSerialized] public RectTransform _rectTransform;
    public Image topImage;
    public Image bottomImage;
    public bool isMainBar;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (infectedCharacter is Sauce)
        {
            slider.value = 1;
            return; 
        }
        if (!isMainBar)
        {
            if (!infectedCharacter.isInfected())
            {
                hide();
            }
            else
            {
                if (infectedCharacter != Player.instance.character)
                { // don't display secondary bars while in host
                    show();
                }
            }
        }

        

        slider.value = infectedCharacter.getInfectionFrac();
    }

    private void show()
    {
        topImage.enabled = true;
        bottomImage.enabled = true;
    }

    public void hide()
    {
        topImage.enabled = false;
        bottomImage.enabled = false;
    }
}
