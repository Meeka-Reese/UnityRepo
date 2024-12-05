using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCollide : MonoBehaviour
{
    public bool pulse;
   [SerializeField] private PulseButton _pulseButton;
    
    // Start is called before the first frame update
    void Start()
    {
        
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

            if (pulse)
            { 
                float Ran = UnityEngine.Random.Range(-1, 1) >= 0 ? 1 : -1;
                transform.eulerAngles +=
                    new Vector3(UnityEngine.Random.Range(-90, 90), UnityEngine.Random.Range(-90, 90), UnityEngine.Random.Range(-90, 90));
                GetComponent<Rigidbody>().linearVelocity += transform.right * Ran * Mathf.Sqrt((1 * 1000) / 1);
            }
        }
        
       
    }

   

    
}
