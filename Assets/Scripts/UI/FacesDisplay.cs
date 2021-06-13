using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacesDisplay : MonoBehaviour
{
    public Image display;

    private void Awake()
    {
        display = GetComponent<Image>(); 
    }
  

    // Update is called once per frame
    void Update()
    {
        Character car = Player.instance.character;
        float frac = car.getInfectionFrac();
        if (car.face0 == null)
        {
            Debug.LogError(car + " has no faces sprites");
        }
        if (frac < 0.33f)
        {
            display.sprite = car.face0;
        }else if (frac < 0.66f)
        {
            display.sprite = car.face1;
        }else
        {
            display.sprite = car.face2;
        }
    }
}
