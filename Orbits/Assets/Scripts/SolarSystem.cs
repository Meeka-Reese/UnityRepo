using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    [SerializeField] private float G = 100f;
    private GameObject Sun;
    
    public bool trigger = false;
    [SerializeField] private SliderScriptTime _sliderScriptTime;
    private float Times;
    [SerializeField] private SliderScriptSunx _sliderScripSunx;
    private float Sunx;
    private GameObject[] celestials;
    
    
    // Start is called before the first frame update
    void Start()
    {
       
        celestials = GameObject.FindGameObjectsWithTag("planet");
        Sun = GameObject.Find("Sun");
       
        InitialVelocity();
        
    }

    // Update is called once per frame
    void Update()
    {
        Sunx = _sliderScripSunx.Sunx;
        
        Times = _sliderScriptTime.Time;
        Time.timeScale = Times;

        
        Time.fixedDeltaTime = 0.02f * Time.timeScale; 
        
    


    }

    private void FixedUpdate()
    {
        Gravity();
        Sun.transform.position = new Vector3(Sunx, 0, 0);
    }

    void Gravity()
    {
        foreach (GameObject a in celestials)
        {
            a.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            foreach (GameObject b in celestials)
            {
                if (!a.Equals(b))
                {
                    
                    float m1 = a.GetComponent<Rigidbody>().mass;
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);
                    a.GetComponent<Rigidbody>().AddForce(((b.transform.position - a.transform.position).normalized
                        * (G * (m1 * m2) / (r * r))));
                    

                }
            }
        }
    }

    void InitialVelocity()
    {
        foreach (GameObject a in celestials)
        {
            foreach (GameObject b in celestials)
            {
                if (!a.Equals(b))
                {
                    float Ran = Random.Range(-1, 1) >= 0 ? 1 : -1;
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);
                    a.transform.LookAt(b.transform);
                    a.transform.eulerAngles += new Vector3(Random.Range(-90,90), Random.Range(-90,90), Random.Range(-90,90));
                    a.GetComponent<Rigidbody>().linearVelocity += a.transform.right * Ran * Mathf.Sqrt((G * m2) / r);
                    
                }
            }
        }
        
    }
}
