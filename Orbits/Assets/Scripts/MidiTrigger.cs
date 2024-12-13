using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MidiTrigger : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    
    [SerializeField] private Slider slider;
    
    public float Note = 60;
    private int[] MajIChord = new int[6];
    private int[] MajbVIIChord = new int[6];
    private int[] MajbVIChord = new int[6];
    private int[] DomVChord = new int[6];
    private int[] BNote = new int[4];
    
    private GameObject Sun;
    private bool CoroutineRunning;
    private int voice = 1;
    
    private float AttackTime = 10f;
    private float DecayTime = 600f;
    private float ReleaseTime = 600f;
    private float SustainLevel = 1f;
    private float triggeron;
    private float VoiceNum = 9;
    private int ChordNum;
    

    private void Start()
    {
        mixer.SetFloat("gain", -40);
        mixer.SetFloat("BassGain", -10);
        
        StartCoroutine(Bass());
        slider.onValueChanged.AddListener((v) =>
        {
            ChordNum = Mathf.RoundToInt(v);
        });
        for (int i = 1; i < VoiceNum; i++)
        {
            mixer.SetFloat("A"+i, AttackTime);
            mixer.SetFloat("D"+i, DecayTime);
            mixer.SetFloat("R"+i, ReleaseTime);
            mixer.SetFloat("S"+i, SustainLevel);
            mixer.SetFloat("Trigger" + i, 0);
        }

        mixer.SetFloat("BassTrigger", 0);

        CoroutineRunning = false;
        Sun = GameObject.Find("Sun");   
        mixer.SetFloat("Gain", 0);
        
        //Chord Pitches
        MajIChord[0] = (int)Note;       
        MajIChord[1] = (int)Note + 2;   
        MajIChord[2] = (int)Note + 4;   
        MajIChord[3] = (int)Note + 7;  
        MajIChord[4] = (int)Note + 14;
        MajIChord[5] = (int)Note + 24;
        
        MajbVIIChord[0] = (int)Note - 2;
        MajbVIIChord[1] = (int)Note + 2;
        MajbVIIChord[2] = (int)Note + 5;
        MajbVIIChord[3] = (int)Note + 9;
        MajbVIIChord[4] = (int)Note + 17;
        MajbVIIChord[5] = (int)Note + 22;
        
        MajbVIChord[0] = (int)Note - 4;
        MajbVIChord[1] = (int)Note;
        MajbVIChord[2] = (int)Note + 3;
        MajbVIChord[3] = (int)Note + 7;
        MajbVIChord[4] = (int)Note + 10;
        MajbVIChord[5] = (int)Note + 14;
        
        DomVChord[0] = (int)Note - 5;
        DomVChord[1] = (int)Note - 3;
        DomVChord[2] = (int)Note;
        DomVChord[3] = (int)Note + 5;
        DomVChord[4] = (int)Note + 12;
        DomVChord[5] = (int)Note + 19;
        
        BNote[0] = (int)Note - 12;
        BNote[1] = (int)Note - 14;
        BNote[2] = (int)Note - 16;
        BNote[3] = (int)Note - 17;
        
        
        
        mixer.SetFloat("Trigger1", 0);
        

    }

  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != Sun)
        {
            switch (ChordNum)
            {
                case 0: PlayNote(MajIChord); break;
                case 1: PlayNote(MajbVIIChord); break;
                case 2: PlayNote(MajbVIChord); break;
                case 3: PlayNote(DomVChord); break;
            }
            
        }
    }

    void PlayNote(int[] Chord)
    {
        float ReleaseTime = Random.Range(20, 1000);
        
        if (voice < 9)
        {
            voice++;
        }
        else
        {
            voice = 1;
        }
        
        int RandomNote = Chord[UnityEngine.Random.Range(0, Chord.Length)];
        mixer.SetFloat("NoteNum"+voice, RandomNote);
        mixer.SetFloat("Trigger"+voice, 1);
        mixer.SetFloat("R"+voice, ReleaseTime);
        
            switch (voice)
            {
                case 1: StartCoroutine(Wait1()); break;
                case 2: StartCoroutine(Wait2()); break;
                case 3: StartCoroutine(Wait3()); break;
                case 4: StartCoroutine(Wait4()); break;
            }
        
        Debug.Log(voice);
    }

    IEnumerator Wait1()
    {
        CoroutineRunning = true;
        yield return new WaitForSeconds(.1f);
        CoroutineRunning = false;
        mixer.SetFloat("Trigger"+voice, 0);
        
    }
    IEnumerator Wait2()
    {
        CoroutineRunning = true;
        yield return new WaitForSeconds(.1f);
        CoroutineRunning = false;
        mixer.SetFloat("Trigger"+voice, 0);
        
    }
    IEnumerator Wait3()
    {
        CoroutineRunning = true;
        yield return new WaitForSeconds(.1f);
        CoroutineRunning = false;
        mixer.SetFloat("Trigger"+voice, 0);
        
    }
    IEnumerator Wait4()
    {
        CoroutineRunning = true;
        yield return new WaitForSeconds(.1f);
        CoroutineRunning = false;
        mixer.SetFloat("Trigger"+voice, 0);
        
    }

    IEnumerator Bass()
    {
        
        while (true)
        {
            float FMAmount = Random.Range(0f, 1000f);
            mixer.SetFloat("BassTrigger", 0);
            yield return new WaitForSeconds(2f);
            mixer.SetFloat("BassFM", FMAmount);
            mixer.SetFloat("BassNoteNum", BNote[ChordNum] - 12);
            mixer.SetFloat("BassTrigger", 1);
            yield return new WaitForSeconds(10f);
        }
    }
    
    
}