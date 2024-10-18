using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] private float BlinkSpeed = 0.1f;
    private TextMeshProUGUI Play;

    public bool CoroutineIsRunning = false;
    void Start()
    {
        Play = GetComponent<TextMeshProUGUI>();

    }

   
    private void Update()
    {
        if (!CoroutineIsRunning)
        {
            StartCoroutine(Flash());
        }
    }
    IEnumerator Flash()
    {
        CoroutineIsRunning = true;
        
        while (true)
        {
            Play.enabled = !Play.enabled;
            yield return new WaitForSeconds(BlinkSpeed);
        }
    }
}
