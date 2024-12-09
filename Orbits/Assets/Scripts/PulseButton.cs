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
        // foreach (GameObject a in celestials)
        // {
        //     
        //             
        //             float Ran = Random.Range(-1, 1) >= 0 ? 1 : -1;
        //             
        //             a.transform.eulerAngles +=
        //                 new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
        //             a.GetComponent<Rigidbody>().velocity += a.transform.right * Ran * Mathf.Sqrt((G * 1000) / 1);
        //
        //         
        //     
        // }
        
    }

    IEnumerator Pulse()
    {
        pulse = true;
        yield return new WaitForSeconds(1f);
        pulse = false;
    }
}
