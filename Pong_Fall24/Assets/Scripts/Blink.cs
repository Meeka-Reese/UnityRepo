using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Blink : MonoBehaviour
{
    private TextMeshProUGUI _messagesGui;
    private bool _inCoroutine;
    [SerializeField] private float _blinkRate = .5f;
    
    private void Start()
    {
        _messagesGui = GetComponent<TextMeshProUGUI>();
        
    }

    private void Update() 
    {
        if (!_inCoroutine)
        {
            StartCoroutine(BlinkMenu());
        }
    }

    IEnumerator BlinkMenu()
    {
        _inCoroutine = true;
        _messagesGui.enabled = !_messagesGui.enabled;
        yield return new WaitForSecondsRealtime(_blinkRate);
        _inCoroutine = false;
    }
    
}
