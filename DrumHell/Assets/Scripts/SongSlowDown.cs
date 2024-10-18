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
