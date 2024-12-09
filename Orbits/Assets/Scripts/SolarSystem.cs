using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SolarSystem : MonoBehaviour
{
    public float G = 100f;
    private GameObject Sun;
    
    public bool trigger = false;
    [SerializeField] private SliderScriptTime _sliderScriptTime;
    private float Times;
    [SerializeField] private SliderScriptSunx _sliderScripSunx;
    private float Sunx;
    private GameObject[] celestials;
    public List<GameObject> celestialsList = new List<GameObject>();
    public Vector3 Limit = new Vector3(3000, 3000, 3000);
    
    
    // Start is called before the first frame update
    void Start()
    {
       
        celestials = GameObject.FindGameObjectsWithTag("planet");
        celestialsList.AddRange(celestials);
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
        if (celestialsList.Count >= 30)
        {
            GameObject ObjtoDestroy = celestialsList[15];
            celestialsList.RemoveAt(15);
            Destroy(ObjtoDestroy);
            
        }
        
    


    }

    private void FixedUpdate()
    {
        Gravity();
        Sun.transform.position = new Vector3(Sunx, 0, 0);
    }

    void Gravity()
    {
        foreach (GameObject a in celestialsList)
        {
            a.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            foreach (GameObject b in celestialsList)
            {
                if (!a.Equals(b))
                {
                   
                    if (Mathf.Abs(a.transform.position.x) > Limit.x || Mathf.Abs(a.transform.position.y) > Limit.y || Mathf.Abs(a.transform.position.z) > Limit.z)
                    {
                        // Reverse velocity if any axis is beyond the limit
                        a.GetComponent<Rigidbody>().velocity *= -1;
                        
                    }
                    else
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
    }

    void InitialVelocity()
    {
        foreach (GameObject a in celestialsList)
        {
            foreach (GameObject b in celestialsList)
            {
                if (!a.Equals(b))
                {
                    
                    
                        float Ran = Random.Range(-1, 1) >= 0 ? 1 : -1;
                        float m2 = b.GetComponent<Rigidbody>().mass;
                        float r = Vector3.Distance(a.transform.position, b.transform.position);
                        a.transform.LookAt(b.transform);
                        a.transform.eulerAngles += new Vector3(Random.Range(-90, 90), Random.Range(-90, 90),
                            Random.Range(-90, 90));
                        a.GetComponent<Rigidbody>().velocity += a.transform.right * Ran * Mathf.Sqrt((G * m2) / r);
                    
                    
                }
            }
        }
        
    }
}
