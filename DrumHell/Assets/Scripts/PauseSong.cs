using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSong : MonoBehaviour
{
    private GameBehavior gameBehavior;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip MainTheme;
    [SerializeField] private AudioClip PauseMusic;
    private bool mainTheme = false;
    private bool pauseMusic = false;

    private bool play = true;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (gameBehavior == null)
        {
            gameBehavior = FindObjectOfType<GameBehavior>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        play = GameBehavior.Instance.play;
        if (play && !mainTheme)
        {
            mainTheme = true;
            pauseMusic = false;
            audioSource.clip = MainTheme;
            audioSource.Play();
        }
        else if (!play && !pauseMusic)
        {
            mainTheme = false;
            pauseMusic = true;
            audioSource.clip = PauseMusic;
            audioSource.Play();
        }
        
    }
}
