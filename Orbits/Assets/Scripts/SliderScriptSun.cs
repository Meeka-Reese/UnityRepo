using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderScriptSun : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    private GameObject Sun;
    [SerializeField] private string TextString;
    private bool CorutineRunning = false;
    
 
    public float Mass;

    [SerializeField] private int initValue = 333000;
    // Start is called before the first frame update
    void Start()
    {
        
        Sun = GameObject.Find("Sun");
        
        GameObject[] Celestials = GameObject.FindGameObjectsWithTag("planet");
        
        
       
        
        slider.onValueChanged.AddListener((v) =>
        {
            text.text = v.ToString("0.00");
            Mass = v;
            if (!CorutineRunning)
            {
                StartCoroutine(SliderWait());
            }
        });
        
        slider.value = initValue;
        
        
     
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = Sun.GetComponent<Rigidbody>();
        rb.mass = Mass;
        
        
    }
    IEnumerator SliderWait()
    {
        CorutineRunning = true;
        yield return new WaitForSecondsRealtime(0.5f);
        text.text = TextString;
        CorutineRunning = false;
    }
}