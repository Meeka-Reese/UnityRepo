using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SphereGrow : MonoBehaviour
{
    [SerializeField] float GrowSpeed = 0.5f;
    private SphereCollider _collider;
    [SerializeField] private Material DistortMat;
    private AudioSource _audioSource;
    
    [Header("Samples")]
    [SerializeField] private AudioClip PopSound;
    [SerializeField] private AudioClip CrushSound;
    
    private Vector3 MaxSize = new Vector3(5000, 5000, 5000);
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Material NotifMat;
    [SerializeField] private float AudioTransitionSpeed;
    private Material BlackWhiteMat;
    private float DryMix = 1f;
    private float WetMix = .5f;
    private float Cutoff = 20000f;
    private float Pitch = 1f;
    private bool InitialSize = true;
    public bool SphereKillRunning = false;
    private GameObject EyeL;
    private GameObject EyeR;
    [Header("Eye Materials For Switching")]
    [SerializeField] private Material EyeMat1;
    [SerializeField] private Material EyeMat2;
    

    void Start()
    {
        EyeL = GameObject.Find("EyeL");
        EyeR = GameObject.Find("EyeR");
        transform.localScale = MaxSize;
        _audioSource = GetComponent<AudioSource>();
        _collider = GetComponent<SphereCollider>();
        DistortMat.SetFloat("_Opacity", 0);
        NotifMat.SetFloat("_ColorMult", .2f);
        BlackWhiteMat = gameObject.GetComponent<Renderer>().material;
        EyeL.GetComponent<Renderer>().material = EyeMat1;
        EyeR.GetComponent<Renderer>().material = EyeMat1;
    }
    void Update()
    {
        
        // if (transform.localScale.x < MaxSize.x)
        // {
        //     transform.transform.localScale += Vector3.one * Time.deltaTime * GrowSpeed;
        // }

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("DistortionCollider") && !SphereKillRunning)
        {
            Debug.Log("PlayerCollide");

            VoidEnter();

        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DistortionCollider") && !SphereKillRunning)
        {
           VoidExit();
        }
    }

    private void VoidEnter()
    {
        NotifMat.SetFloat("_ColorMult", 1);
        DistortMat.SetFloat("_Opacity", 1);
        _audioSource.PlayOneShot(PopSound);
        
        EyeL.GetComponent<Renderer>().material = EyeMat2;
        EyeR.GetComponent<Renderer>().material = EyeMat2; 
        //Changing Mat with different render layer so eyes show with distort effect
        DryMix = Mathf.Lerp(DryMix, .5f, Time.deltaTime * AudioTransitionSpeed);
        WetMix = Mathf.Lerp(WetMix, 1, Time.deltaTime * AudioTransitionSpeed);
        Cutoff = Mathf.Lerp(Cutoff, 3000, Time.deltaTime * AudioTransitionSpeed);
        Pitch = Mathf.Lerp(Pitch, .73f, Time.deltaTime * AudioTransitionSpeed);
        
        mixer.SetFloat("DryMixDly", DryMix);
        mixer.SetFloat("WetMixDly", WetMix);
        mixer.SetFloat("CutoffLP", Cutoff);
        mixer.SetFloat("PitchShift", Pitch);
    }

    private void VoidExit() 
    {
        NotifMat.SetFloat("_ColorMult", .2f);
        DistortMat.SetFloat("_Opacity", 0);
        _audioSource.PlayOneShot(PopSound);
        EyeL.GetComponent<Renderer>().material = EyeMat1;
        EyeR.GetComponent<Renderer>().material = EyeMat1;
        DryMix = Mathf.Lerp(DryMix, 1, Time.deltaTime * AudioTransitionSpeed);
        WetMix = Mathf.Lerp(WetMix, 0, Time.deltaTime * AudioTransitionSpeed);
        Cutoff = Mathf.Lerp(Cutoff, 20000, Time.deltaTime * AudioTransitionSpeed);
        Pitch = Mathf.Lerp(Pitch, 1, Time.deltaTime * AudioTransitionSpeed);
        mixer.SetFloat("DryMixDly", DryMix);
        mixer.SetFloat("WetMixDly", WetMix);
        mixer.SetFloat("CutoffLP", Cutoff);
        mixer.SetFloat("PitchShift", Pitch);
        
        
    }
    public IEnumerator SphereKill()
    {
        _audioSource.PlayOneShot(CrushSound);
        float NoiseAmount = 0;
        SphereKillRunning = true;
        Debug.Log("SphereKill");
        DistortMat.SetFloat("_Opacity", 0);
        // _audioSource.PlayOneShot(PopSound);
        Vector3 shrinkAmount = Vector3.one * (GrowSpeed / 48.66f);
        while (transform.transform.localScale.x > .05)
        {
            
            transform.transform.localScale -= shrinkAmount;
            if (transform.transform.localScale.x > 4500 && transform.transform.localScale.x < 5000)
            {
                NoiseAmount += .005f;
                BlackWhiteMat.SetFloat("_NoiseAmount", NoiseAmount);
            }
            if (transform.transform.localScale.x < 4500)
            {
                
                NoiseAmount += .01f;
                BlackWhiteMat.SetFloat("_NoiseAmount", NoiseAmount);
            }

            yield return null;
        }

        if (transform.transform.localScale.x < .05)
        {
            Destroy(gameObject);
        }
        
    }
}
