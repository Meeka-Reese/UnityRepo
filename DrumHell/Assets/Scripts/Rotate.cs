using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public AudioSource _source;
    public float rotMax;
    public float rotLim;
    // [SerializeField] AudioClip Kick;
    public LoudnessMonitor LoudnessMonitor;
    public static float Loudness;
    public float MoveSize = 3;
    public Powerup _powerup;
    private bool PowerupActive;
    [SerializeField] float PowerUpTime = 5f;
    private bool IsCoroutineRunning = false;
    private Quaternion Pmax;  
    private Quaternion Pmin; 
    

    
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();

        _powerup.PowerupActivated = false;

    }

    // Update is called once per frame
    void Update()
    {
        PowerupActive = _powerup.PowerupActivated;
        _source.pitch = PowerupActive == true ? .5f : 1f;
        if (PowerupActive && !IsCoroutineRunning)
        {
            StartCoroutine(PowerUpTimer());
        }
        // Debug.Log(PowerupActive);
        
        
        Quaternion Pmax = new Quaternion(transform.rotation.x, transform.rotation.y, rotMax, transform.rotation.w);
        Quaternion Pmin = new Quaternion(transform.rotation.x, transform.rotation.y, rotLim, transform.rotation.w);
        Loudness = (LoudnessMonitor.GetVolume(_source.timeSamples, _source.clip));
        transform.rotation = Quaternion.Lerp(Pmin, Pmax, (Loudness));
        Debug.Log(Loudness);

    }
    IEnumerator PowerUpTimer()
    {
        IsCoroutineRunning = true;
        
        yield return new WaitForSeconds(PowerUpTime);
        
        _powerup.PowerupActivated = false;
        
    }

    
}