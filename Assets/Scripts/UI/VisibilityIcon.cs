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
    private InfectionBar infBar;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        infBar = GetComponentInChildren<InfectionBar>();
        infBar.infectedCharacter = controller.character;
        infBar.transform.parent = UIManager.instance.characterInfectionBars.transform;
    }

    private void Update()
    {
        try
        {
            _rectTransform.position = controller.transform.position + new Vector3(0, 1, 0);
            infBar._rectTransform.position = controller.transform.position + new Vector3(0, 1, 0);
        }
        catch (MissingReferenceException e)
        {
            Destroy(gameObject);
            Destroy(infBar.gameObject);
        }
    }

    public void setText(string newText)
    {
        if (newText != "")
        {
            infBar.hide();
        }
        text.text = newText;
    }
}