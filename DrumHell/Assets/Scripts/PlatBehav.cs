using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatBehav : MonoBehaviour
{
    public AudioSource _source;
    public float xMax;
    public float xLim;
    // [SerializeField] AudioClip Kick;
    public LoudnessMonitor LoudnessMonitor;
    public static float Loudness;
    public float MoveSize = 3;
    

    
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Pmax = new Vector3(xMax, transform.position.y, transform.position.z);
        Vector3 Plim = new Vector3(xLim, transform.position.y, transform.position.z);
        Loudness = (LoudnessMonitor.GetVolume(_source.timeSamples, _source.clip)) * 10f;
        transform.position = Vector3.Lerp(Plim, Pmax, (Loudness)* MoveSize);

    }

    
}
