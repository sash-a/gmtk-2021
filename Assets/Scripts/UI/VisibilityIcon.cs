using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisibilityIcon : MonoBehaviour
{
    [NonSerialized] public Ai controller;
    private TextMeshPro text;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        try
        {
            GetComponent<RectTransform>().position = controller.transform.position + new Vector3(0, 1, 0);
        }
        catch (MissingReferenceException e)
        {
            Destroy(gameObject);
        }
    }

    public void setText(string empty)
    {
        text.text = empty;
    }
}
