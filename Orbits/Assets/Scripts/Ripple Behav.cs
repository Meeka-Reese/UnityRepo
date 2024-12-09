using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class RippleBehav : MonoBehaviour
{
    
    public GameObject _soundTrigger; 
    [SerializeField] private ParticleSystem particle;
    private Rigidbody rb;
    private AudioSource audio;
    [SerializeField] private AudioClip Pluck1;
    [SerializeField] private AudioClip Pluck2;
    public float[] Pitches = new float[24];
    [SerializeField] private int index = 0;
    private float[] _noteMultipliers = new float[24];
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        

        InitNoteMultipliers();
        _soundTrigger = GameObject.Find("SoundTrigger");


    }
    void InitNoteMultipliers()
    {
        // The note multipliers are computed using the 12th root of the
        // scale step.
        float twelfthRoot = Mathf.Pow(2.0f, 1.0f / 12.0f);

        _noteMultipliers = _noteMultipliers
            .Select((_, i) => Mathf.Pow(twelfthRoot, i))
            .ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("trigger"))
        {
            
            particle.Play();
            StartCoroutine(PlaySample());
        }

        
    

    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("trigger"))
        {
            particle.Stop();
        }
    }

    IEnumerator PlaySample()
    {
        audio.clip = Pluck1;
        audio.Play();
        audio.pitch = _noteMultipliers[index];
        
        
        yield return new WaitForSeconds(.1f);
        
    }
}
