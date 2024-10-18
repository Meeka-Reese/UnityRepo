using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private float _duration;
    private TextMeshProUGUI _clockGui;
    public float _elapsedTime;

    public float ElapsedTime
    {
        get => _elapsedTime;
        set
        {
            _elapsedTime = value;
            int minutes = Mathf.FloorToInt(_elapsedTime / 60.0f);
            int seconds =Mathf.FloorToInt(_elapsedTime % 60.0f);

            _clockGui.text = $"{minutes:00}:{seconds:00}";
        }
    }

    private void Start()
    {
        _clockGui = GetComponent<TextMeshProUGUI>();
    }
    
    void Update()
    {
        ElapsedTime += Time.deltaTime;
        if (_elapsedTime >= _duration)
        {
            
            ElapsedTime -= _duration;
        }
    }
}