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
    public BadPowerUp _badpowerup;
    private bool PowerupActive;
    private bool BadPowerupActive;
    [SerializeField] float PowerUpTime = 5f;
    private bool IsCoroutineRunning = false;
    [SerializeField] private GameBehavior GameBehavior;
    private bool play = true;
    
    

    
    // Start is called before the first frame update
    void Start()
    {
        
        
        _source = GetComponent<AudioSource>();
        if (GameBehavior == null)
        {
            GameBehavior = FindObjectOfType<GameBehavior>();
        }

        _powerup.PowerupActivated = false;
        _source.pitch = 1;

    }

    // Update is called once per frame
    void Update()
    {
        play = GameBehavior.Instance.play;
        
        PowerupActive = _powerup.PowerupActivated;
        BadPowerupActive = _badpowerup.BadPowerupActivated;
        // _source.pitch = BadPowerupActive == true ? 2f : 1f;
        if (BadPowerupActive == true)
        {
            _source.pitch = 2f;
        }
        else if (PowerupActive == true)
        {
            _source.pitch = .5f;
        }
        if (_badpowerup && !IsCoroutineRunning)
        {
            StartCoroutine(BadPowerUpTimer());
        }
        // _source.pitch = PowerupActive == true ? .5f : 1f;
        if (PowerupActive && !IsCoroutineRunning)
        {
            StartCoroutine(PowerUpTimer());
        }
        // Debug.Log(PowerupActive);

        if (play)
        {
            Vector3 Pmax = new Vector3(xMax, transform.position.y, transform.position.z);
            Vector3 Plim = new Vector3(xLim, transform.position.y, transform.position.z);
            Loudness = (LoudnessMonitor.GetVolume(_source.timeSamples, _source.clip)) * 10f;
            transform.position = Vector3.Lerp(Plim, Pmax, (Loudness) * MoveSize);
        }
        else
        {
            // Debug.Log("Not playing anymore");
        }

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
        
        yield return new WaitForSeconds(PowerUpTime);
        
        _badpowerup.BadPowerupActivated = false;
       
        _source.pitch = 1;
    }

    
}
