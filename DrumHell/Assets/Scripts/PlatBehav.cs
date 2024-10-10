using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatBehav : MonoBehaviour
{
    public AudioSource _source;
    public float yMax;
    public float yLim;
    // [SerializeField] AudioClip Kick;
    public LoudnessMonitor LoudnessMonitor;
    public static float Loudness;
    

    
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Pmax = new Vector3(transform.position.x, yMax, transform.position.z);
        Vector3 Plim = new Vector3(transform.position.x, yLim, transform.position.z);
        Loudness = (LoudnessMonitor.GetVolume(_source.timeSamples, _source.clip)) * 10f;
        transform.position = Vector3.Lerp(Plim, Pmax, Loudness*Time.deltaTime);

    }

    
}
