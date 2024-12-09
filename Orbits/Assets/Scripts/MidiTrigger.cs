using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MidiTrigger : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    public float Note = 60;
    private int[] MajChord = new int[5];
    private GameObject Sun;
    private bool CoroutineRunning;
    private int voice = 1;

    private void Start()
    {
        CoroutineRunning = false;
        Sun = GameObject.Find("Sun");
        mixer.SetFloat("gain", 0);
        MajChord[0] = (int)Note;       
        MajChord[1] = (int)Note + 4;   
        MajChord[2] = (int)Note + 7;   
        MajChord[3] = (int)Note + 11;  
        MajChord[4] = (int)Note + 14;  
        
    }

    private void Update()
    {
        mixer.SetFloat("count", voice);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != Sun)
        {
            PlayNote(MajChord);
        }
    }

    void PlayNote(int[] Chord)
    {
        if (voice < 4)
        {
            voice++;
        }
        else
        {
            voice = 1;
        }
        
        int RandomNote = Chord[UnityEngine.Random.Range(0, Chord.Length)];
        mixer.SetFloat("notenum", RandomNote);
        mixer.SetFloat("trigger", 1);
        if (!CoroutineRunning)
        {
            StartCoroutine(Wait());
        }
        Debug.Log(voice);
    }

    IEnumerator Wait()
    {
        CoroutineRunning = true;
        yield return new WaitForSeconds(.25f);
        mixer.SetFloat("trigger", 0);
        
    }
    
    
}