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
    private bool BadPowerupActive;
    public BadPowerUp _badpowerup;
    

    
    // Start is called before the first frame update
    void Start()
    
    {
        _source = GetComponent<AudioSource>();
        _powerup.PowerupActivated = false;
        _source.pitch = 1;
    }

    // Update is called once per frame
    void Update()
    {
        PowerupActive = _powerup.PowerupActivated;
        if (BadPowerupActive == true)
        {
            _source.pitch = 2f;
        }
        else if (PowerupActive == true)
        {
            _source.pitch = .5f;
        }
        if (PowerupActive && !IsCoroutineRunning)
        {
            StartCoroutine(PowerUpTimer());
        }
        if (_badpowerup && !IsCoroutineRunning)
        {
            StartCoroutine(BadPowerUpTimer());
        }
        // Debug.Log(PowerupActive);
        
        
        Quaternion Pmax = new Quaternion(transform.rotation.x, transform.rotation.y, rotMax, transform.rotation.w);
        Quaternion Pmin = new Quaternion(transform.rotation.x, transform.rotation.y, rotLim, transform.rotation.w);
        Loudness = (LoudnessMonitor.GetVolume(_source.timeSamples, _source.clip));
        transform.rotation = Quaternion.Lerp(Pmin, Pmax, (Loudness));
        

    }
    IEnumerator PowerUpTimer()
    {
        IsCoroutineRunning = true;
        
        yield return new WaitForSeconds(PowerUpTime);
        
        _powerup.PowerupActivated = false;
        _source.pitch = 1;
    }
    IEnumerator BadPowerUpTimer()
    {
        IsCoroutineRunning = true;
        Debug.Log("Waiting");
        yield return new WaitForSeconds(PowerUpTime);
        
        _badpowerup.BadPowerupActivated = false;
        Debug.Log("Waiting Done");
        _source.pitch = 1;
    }

    
}