using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SynthCollide : MonoBehaviour
{
    
    [SerializeField] AudioMixer Synthmixer;
    public bool CoroutineIsRunning = false;
    void Start()
    {
        // Test if the parameter exists by trying to get its value
        float value;
        if (Synthmixer.GetFloat("Volume", out value))
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
        
        Debug.Log("SynthPlay");
        CoroutineIsRunning = true;
        Synthmixer.SetFloat("adsr_trigger", 1);
        yield return new WaitForSeconds(0.1f);
        Synthmixer.SetFloat("adsr_trigger", 0);
        CoroutineIsRunning = false;
    }
}
