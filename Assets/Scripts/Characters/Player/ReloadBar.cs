using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
    public Slider slider;
    private Image image;
    public GameObject fill;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (Player.instance.character is Ranged ranged)
        {
            image.enabled = true;
            fill.SetActive(true);

            float timeSinceShot = Time.time - ranged.lastFire;
            if (timeSinceShot > ranged.firePeriod)
            {
                slider.value = 1;
                return;
            }

            slider.value = timeSinceShot / ranged.firePeriod;
        }
        else
        {
            image.enabled = false;
            fill.SetActive(false);
        }
        
    }
}
