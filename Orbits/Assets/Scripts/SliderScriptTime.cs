using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderScriptTime : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    
    
 
    public float Time;

    [SerializeField] private int initValue = 200;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = initValue;
        
        
        
        
        
        slider.onValueChanged.AddListener((v) =>
        {
            text.text = v.ToString("0.00");
            Time = v;
        });
     
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}