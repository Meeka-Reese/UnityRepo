using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCollide : MonoBehaviour
{
    public bool pulse;
   private PulseButton _pulseButton;
   
   
    
    // Start is called before the first frame update
    void Start()
    {
        _pulseButton = FindObjectOfType<PulseButton>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        pulse = _pulseButton.pulse;
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("planet"))
        {

            // if (pulse)
            // { 
                float Ran = UnityEngine.Random.Range(-1, 1) >= 0 ? 1 : -1;
                transform.eulerAngles +=
                    new Vector3(UnityEngine.Random.Range(-90, 90), UnityEngine.Random.Range(-90, 90), UnityEngine.Random.Range(-90, 90));
                GetComponent<Rigidbody>().velocity += transform.right * Ran * Mathf.Sqrt((1 * 1000) / 1);
            // }
        }
        
       
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.CompareTag("bounds"))
    //     {
    //         Rigidbody rb = GetComponent<Rigidbody>();
    //         if (rb != null)
    //         {
    //
    //
    //             Boundshit = true;
    //             Debug.Log("Velo Rev");
    //         }
    //     }
    // }
}
