using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class SoundHandler : MonoBehaviour
{
    public int Index;
    public bool Play = false;
    public float Pitch = 1.0f;
  
    
    public void ChooseSong(int SongNumber)
    {
        Index = SongNumber;
    }

    public void PlaySong()
    {
        Play = true;
    }

    public void StopSong()
    {
        Play = false;
    }

    public void DayCore()
    {
        Pitch = .75f;
    }

    public void NightCore()
    {
        Pitch = 1.25f;
    }

    public void TodayCore()
    {
        Pitch = 1.0f;
    }

    
    
}
