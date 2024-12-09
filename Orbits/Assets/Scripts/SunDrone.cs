using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDrone : MonoBehaviour
{
    private ParticleSystem particle;
    private AudioSource audio;
    [SerializeField] private AudioClip Drone1;
    private bool Coroutinerunning = false;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Coroutinerunning)
        {
            StartCoroutine(Drone());
        }
        
    }

    IEnumerator Drone()
    {
        Coroutinerunning = true;
        audio.clip = Drone1;
        audio.Play();
        particle.Play();
        yield return new WaitForSeconds(1f);
        particle.Stop();
        yield return new WaitForSeconds(4f);
        Coroutinerunning = false;
    }
}
