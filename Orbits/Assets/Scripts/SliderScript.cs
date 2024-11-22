using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    private GameObject[] Orbits;
    private List<GameObject> orbitList = new List<GameObject>();
 
    public float Mass;

    [SerializeField] private int initValue = 200;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = initValue;
        GameObject[] Celestials = GameObject.FindGameObjectsWithTag("planet");
        
        
        foreach (GameObject celestial in Celestials)
        {
            if (celestial.name.StartsWith("orbit ("))
            {
                orbitList.Add(celestial);
            }
        }
        
     slider.onValueChanged.AddListener((v) =>
     {
       text.text = v.ToString("0.00");
       Mass = v;
       
     });
     
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject orbit in orbitList)
        {
            float sizemult = (Mass / 50);
            sizemult = Math.Clamp(sizemult, 25, 80);
            Vector3 size = new Vector3(sizemult, sizemult, sizemult);
            orbit.transform.localScale = size;
            
            Rigidbody rb = orbit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.mass = Mass;
            }
        }
        
    }
}
