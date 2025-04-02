using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;


public class SoundHandler : MonoBehaviour
{
    public int Index;
    public bool Play = false;
    public float Pitch = 1.0f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioClip HarpSong;
    [SerializeField] private AudioClip HarpSongAlt;
    [SerializeField] private AudioMixer mixer;
    public float Pitch2 = 1.0f;
    public bool Night = false;
    [SerializeField] private float fadeDuration = 0.25f;

    private void Start()
    {
        Pitch2 = 1f;
        audioSource.clip = HarpSong;
        audioSource2.clip = HarpSongAlt;
        audioSource.loop = true;
        audioSource2.loop = true;
        audioSource.Play();
        audioSource2.Play();
        Night = false;
        mixer.SetFloat("BgAltVolume", -80.0f);
        mixer.SetFloat("BgVolume", 0.0f);
    }

    private void Update()
    {
        audioSource.pitch = Pitch2;
        audioSource2.pitch = Pitch2;
        
    }

    public void ChooseSong(int SongNumber)
    {
        Index = SongNumber;
        PlaySong();
    }

    public void PlaySong()
    {
        Play = true;
        Pitch2 = 0.0f;

    }

    public void StopSong()
    {
        Play = false;
        Pitch2 = 1.0f;
        Night = false;
        
    }

    public void DayCore()
    {
        Pitch = .75f;
        Night = false;
    }

    public void NightCore()
    {
        Pitch = 1.25f;
        Night = true;
    }

    public void TodayCore()
    {
        Pitch = 1.0f;
        Night = false;
    }

    public void ChangeTrigger(String ParamName, String ParamName2)
    {
        StartCoroutine(Change(ParamName, ParamName2));
    }

    private IEnumerator Change(string paramName1, string paramName2)
    {
        float startVolume1, startVolume2;
        mixer.GetFloat(paramName1, out startVolume1);
        mixer.GetFloat(paramName2, out startVolume2);

        float targetVolume1 = -80f;
        float targetVolume2 = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            
            mixer.SetFloat(paramName1, Mathf.Lerp(startVolume1, targetVolume1, t));
            mixer.SetFloat(paramName2, Mathf.Lerp(startVolume2, targetVolume2, t));

            yield return null;
        }

        mixer.SetFloat(paramName1, targetVolume1);
        mixer.SetFloat(paramName2, targetVolume2);
    }
}

    

    
    

