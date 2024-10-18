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
    public Powerup _powerup;
    private bool PowerupActive;
    [SerializeField] float PowerUpTime = 5f;
    private bool IsCoroutineRunning = false;
    

    
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
        
        
        Vector3 Pmax = new Vector3(xMax, transform.position.y, transform.position.z);
        Vector3 Plim = new Vector3(xLim, transform.position.y, transform.position.z);
        Loudness = (LoudnessMonitor.GetVolume(_source.timeSamples, _source.clip)) * 10f;
        transform.position = Vector3.Lerp(Plim, Pmax, (Loudness)* MoveSize);

    }
    IEnumerator PowerUpTimer()
    {
        IsCoroutineRunning = true;
        Debug.Log("Waiting");
        yield return new WaitForSeconds(PowerUpTime);
        
        _powerup.PowerupActivated = false;
        Debug.Log("Waiting Done");
    }

    
}
