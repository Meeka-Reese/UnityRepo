using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class exposedparam : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mixer.SetFloat("Volume", .25f);
    }
}
