using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisibilityIcon : MonoBehaviour
{
    [NonSerialized] public Ai controller;
    private TextMeshPro text;
    private RectTransform _rectTransform;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        try
        {
            _rectTransform.position = controller.transform.position + new Vector3(0, 1, 0);
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