using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SynthCollide : MonoBehaviour
{
    
    [SerializeField] AudioMixer Synthmixer;
    public bool CoroutineIsRunning = false;
    private float randomf;
    private float randomf2;
    void Start()
    {
        // Test if the parameter exists by trying to get its value
        float value;
        if (Synthmixer.GetFloat("adsr", out value))
        {
            Debug.Log("Parameter adsr_trigger' found! Current value: " + value);
        }
        else
        {
            Debug.LogError("Parameter 'adsr_trigger' does not exist or is not exposed in the AudioMixer!");
        }
    }
    
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("floor"))
        {
            
            if (!CoroutineIsRunning)
            {
                StartCoroutine(SynthPlay());
            }
        }
    }

    IEnumerator SynthPlay()
    {
        randomf = Random.Range(200f, 3000f);
        randomf2 = Random.Range(200f, 3000f);
        Debug.Log("SynthPlay");
        CoroutineIsRunning = true;
        Synthmixer.SetFloat("adsr", 1);
        Synthmixer.SetFloat("frequency", randomf);
        Synthmixer.SetFloat("frequency2", randomf2);
        yield return new WaitForSeconds(0.1f);
        Synthmixer.SetFloat("adsr", 0);
        CoroutineIsRunning = false;
    }
}
