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
    [SerializeField] private float delay;
    public Powerup _powerup;
    private bool PowerupActive;
    [SerializeField] float PowerUpTime = 5f;
    private bool IsCoroutineRunning = false;
    private GameBehavior GameBehavior;
    private bool play = true;

   
    void Start()
    {
        
        GameBehavior = FindObjectOfType<GameBehavior>();
        _source = GetComponent<AudioSource>();
        _source.loop = true; 
        PlaySampleWithDelay(delay);
        _powerup.PowerupActivated = false;
    }

    
    void Update()
    {
        play = global::GameBehavior.Instance.play;
        PowerupActive = _powerup.PowerupActivated;
        _source.pitch = PowerupActive == true ? .5f : 1f;
        if (PowerupActive && !IsCoroutineRunning)
        {
            StartCoroutine(PowerUpTimer());
        }

        if (play)
        {
            Vector3 Pmax = new Vector3(xMax, transform.position.y, transform.position.z);
            Vector3 Plim = new Vector3(xLim, transform.position.y, transform.position.z);
            Loudness = (LoudnessMonitor.GetVolume(_source.timeSamples, _source.clip)) * 10f;
            transform.position = Vector3.Lerp(Plim, Pmax, (Loudness) * MoveSize);
        }
    }
    void PlaySampleWithDelay(float delay)
    {
        double startTime = AudioSettings.dspTime + delay;
        _source.PlayScheduled(startTime);
    }


    // IEnumerator StepPlay()
    // {
    //     yield return new WaitForSecondsRealtime(DelayTime);
    //     _source.Play();  
    //     StopCoroutine(StepPlay());
    // }
    IEnumerator PowerUpTimer()
    {
        IsCoroutineRunning = true;
        Debug.Log("Waiting");
        yield return new WaitForSeconds(PowerUpTime);
        
        _powerup.PowerupActivated = false;
        Debug.Log("Waiting Done");
    }
}
