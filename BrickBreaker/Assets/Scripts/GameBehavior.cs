using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;
    [SerializeField] Player[] _players = new Player[1];
    public int AddScore = 0;
    [SerializeField] int WinAmount = 10;
    [SerializeField] private TextMeshProUGUI WinText;
    [SerializeField] private GameObject Ball;
    [SerializeField] private GameObject Pause;
    public Utilities.GameplayState State = Utilities.GameplayState.Play;
    public bool ResetGame = false;
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _source.pitch = 1.0f;
        Pause.SetActive(false);
        Ball.SetActive(true);
        WinText.enabled = false;
        if (Instance != null && Instance != this)
        {
            //destroy duplicates
            Destroy(this);
        }
        else
        {
            //Runs first time around
            Instance = this;
            // Uncomment on level based games
            // DontDestroyOnLoad(gameObject);
        }
    }
    //Belongs to class not instance
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (State == Utilities.GameplayState.Play)
            {
                Pause.SetActive(true);
                State = Utilities.GameplayState.Pause;
                _source.pitch = .39f;

            }
            else if (State == Utilities.GameplayState.Pause)
            {
                Pause.SetActive(false);
                State = Utilities.GameplayState.Play;
                _source.pitch = 1.0f;
            }
            
        }
        foreach (Player p in _players)
        {
            p.Score = AddScore;
            if (p.Score >= WinAmount)
            {
                WinText.enabled = true;
                Ball.SetActive(false);
            }
        }

        
    }
}