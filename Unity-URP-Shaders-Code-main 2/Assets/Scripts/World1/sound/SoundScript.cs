using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class SoundScript : MonoBehaviour
{
    [SerializeField] private AudioClip[] _clips;
    private AudioSource audioSource;
    [SerializeField] private SoundHandler soundHandler;
    private bool Play = false;
    private int Index;
    private int LastIndex = -1;
    private float CurrentPitch = 1.0f;
    private bool AudioPlaying = false;
    
    private void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 0.0f;
        audioSource.clip = _clips[0];
        soundHandler.Pitch = 1.0f;
        audioSource.Play();
        
    }

    private void Update()
    {
        
       
        Index = soundHandler.Index;
        Play = soundHandler.Play;
        if (Play)
        {
            
            if (!AudioPlaying)
            {
                
                audioSource.pitch = CurrentPitch;
                AudioPlaying = true;
            }

            if (AudioPlaying)
            {
                audioSource.pitch = soundHandler.Pitch;
                CurrentPitch = soundHandler.Pitch;
            }
        }
        else if (!Play)
        {
            if (AudioPlaying)
            {
                
                audioSource.pitch = 0.0f;
                AudioPlaying = false;
            }
        }

        if (LastIndex != Index)
        {
            audioSource.clip = _clips[Index];
            LastIndex = Index;
        }

        


    }
    

    
}
