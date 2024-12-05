using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseButton : MonoBehaviour
{
    private GameObject[] celestials;
    [SerializeField] private float G = 100f;
    public bool pulse;
    
    

    // Start is called before the first frame update
    void Start()
    {
        celestials = GameObject.FindGameObjectsWithTag("planet");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPulse()
    {
        
        StartCoroutine(Pulse());
        Debug.Log("pulse");
        
        
    }

    IEnumerator Pulse()
    {
        pulse = true;
        yield return new WaitForSeconds(1f);
        pulse = false;
    }
}
// markov chains rnbo spacialize audio to camera UDP max
