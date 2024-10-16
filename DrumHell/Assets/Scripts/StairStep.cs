using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairStep : MonoBehaviour
{
    public AudioSource _source;
    public float xMax;
    public float xLim;
    public LoudnessMonitor LoudnessMonitor;
    public static float Loudness;
    public float MoveSize = 3;
    [SerializeField] private float DelayTime;

   
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.loop = true; 
        StartCoroutine(StepPlay());
    }

    
    void Update()
    {
        Vector3 Pmax = new Vector3(xMax, transform.position.y, transform.position.z);
        Vector3 Plim = new Vector3(xLim, transform.position.y, transform.position.z);
        Loudness = (LoudnessMonitor.GetVolume(_source.timeSamples, _source.clip)) * 10f;
        transform.position = Vector3.Lerp(Plim, Pmax, (Loudness) * MoveSize);
    }

    IEnumerator StepPlay()
    {
        yield return new WaitForSecondsRealtime(DelayTime);
        _source.Play();  
    }
}
