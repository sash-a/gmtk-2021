using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour
{
    public Image display;
    public Sprite gun;
    public Sprite fist;
    private void Awake()
    {
        display = GetComponent<Image>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.instance.character is Sauce)
        {
            display.color = new Color(0,0,0,0);
        }

        if (Player.instance.character is Ranged)
        {
            display.sprite = gun;
            display.color = Color.white;
            display.gameObject.transform.localScale = new Vector3(2, 2, 2);
        }
        if (Player.instance.character is Melee)
        {
            display.sprite = fist;
            display.color = Color.white;
            display.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }
}
