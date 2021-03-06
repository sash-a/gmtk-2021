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
            float size = 2f;
            display.sprite = gun;
            display.color = Color.white;
            display.gameObject.transform.localScale = new Vector3(size, size, size);
        }
        if (Player.instance.character is Melee)
        {
            float size = 3.3f;
            display.sprite = fist;
            display.color = Color.white;
            display.gameObject.transform.localScale = new Vector3(size, size, size);
        }
    }
}
