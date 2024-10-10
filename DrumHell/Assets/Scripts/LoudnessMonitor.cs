using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoudnessMonitor : MonoBehaviour
{
    public int SampleWindow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float GetVolume(int clipPosition, AudioClip clip)
    {
        int StartPosition = clipPosition - SampleWindow;
        
        if (StartPosition < 0)
        {
            return 0;
        }
        float[] waveData = new float[SampleWindow];
        clip.GetData(waveData, StartPosition);
        //Get Loudness
        //https://www.youtube.com/watch?v=dzD0qP8viLw&list=PLkmEDZ0E8yk5x4hV5hUS4tGfuNurF54Ql&ab_channel=ValemTutorials
        float volume = 0f;

        for (int i = 0; i < waveData.Length; i++)
        {
            volume += Mathf.Abs(waveData[i]);
        }
        return volume / SampleWindow;
        
        
        
    }
}
