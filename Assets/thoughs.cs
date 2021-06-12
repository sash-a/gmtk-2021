using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class thoughs : MonoBehaviour
{

    public string infectedthought = "Ugly";

    public TMP_Text thoughts;

    void Start(){
        gameObject.SetActive(false);

    }

    void Update(){
        thoughts.text = infectedthought;
        //every 5 seconds infectedthought = from database [random int]
        
    }
    
}
