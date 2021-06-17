using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteController : MonoBehaviour
{
    public GameObject torso;
    public GameObject legs;

    [NonSerialized] public Animator torsoAnimator;
    [NonSerialized] public Animator legsAnimator;

    private void Awake()
    {
        torsoAnimator = torso.GetComponent<Animator>();
        legsAnimator = legs.GetComponent<Animator>();
    }
}
