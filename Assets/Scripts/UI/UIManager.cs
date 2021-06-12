using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    
    

    public float ZomHealth = 1f;
    
    // Start is called before the first frame update
    

    public Slider slider;
    public void SetZomHealth()
    {
        
        slider.value = ZomHealth;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ZomHealth =  0.3f; //(time.time-timeOfInfection)/infectionTime;
        SetZomHealth();
    
    }
}
