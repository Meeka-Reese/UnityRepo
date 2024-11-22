using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSlowDown : MonoBehaviour
{
    public Powerup _powerup;
    private bool PowerupActive;
    [SerializeField] float PowerUpTime = 5f;
    private bool IsCoroutineRunning = false;
    public AudioSource _source;
    private bool BadPowerupActive;
    public BadPowerUp _badpowerup;
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.pitch = 1;

        _powerup.PowerupActivated = false;
        _badpowerup.BadPowerupActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
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
        else
        {
            _source.pitch = 1f;
        }
        if (_badpowerup && !IsCoroutineRunning)
        {
            StartCoroutine(BadPowerUpTimer());
        }
        PowerupActive = _powerup.PowerupActivated;
        // _source.pitch = PowerupActive == true ? .5f : 1f;
        if (PowerupActive && !IsCoroutineRunning)
        {
            StartCoroutine(PowerUpTimer());
        }
    }
    IEnumerator PowerUpTimer()
    {
        IsCoroutineRunning = true;
        
        yield return new WaitForSeconds(PowerUpTime);
        
        _powerup.PowerupActivated = false;
        
        _source.pitch = 1;
        PowerupActive = false;
    }
    IEnumerator BadPowerUpTimer()
    {
        IsCoroutineRunning = true;
        
        yield return new WaitForSeconds(PowerUpTime);
        
        _badpowerup.BadPowerupActivated = false;
        
        _source.pitch = 1;
        BadPowerupActive = false;
    }
}
