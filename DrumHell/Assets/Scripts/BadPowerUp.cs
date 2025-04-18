using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BadPowerUp : MonoBehaviour
{
    public bool CoroutineIsRunning = false;
    public bool BadPowerupActivated = false;
    public SpriteRenderer spriteRenderer;
    [SerializeField] private float BlinkSpeed = 0.1f;
    
    private void start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (!CoroutineIsRunning)
        {
            StartCoroutine(Blink());
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            BadPowerupActivated = true;
            Destroy(gameObject);



        }

    }

    IEnumerator Blink()
    {
        CoroutineIsRunning = true;
        
        while (true)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(BlinkSpeed);
        }
    }

    
}