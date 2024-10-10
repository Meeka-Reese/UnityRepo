using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;
    public float InitBallSpeed = 5.0f;
    public float BallSpeedIncrement = 100f;
    [SerializeField] int _scoreToVictory = 3;
    [SerializeField] Player[] _players = new Player[2];
    public Utilities.GameplayState State = Utilities.GameplayState.Play;
    [SerializeField] private TextMeshProUGUI _messages;

    private void Awake()
    {
        
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
    private void Start()
    {
        ResetGame();
        State = Utilities.GameplayState.Play;
        _messages.enabled = false;
    }

    private void Update()
    {
        SwitchState();
        
    }

    private void SwitchState()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (State == Utilities.GameplayState.Play)
            {
                State = Utilities.GameplayState.Pause;
                _messages.text = "Pause";
                _messages.enabled = true;
            }
            else
            {
                State = Utilities.GameplayState.Play;
                _messages.enabled = false;
            }
        }
        
    }

    public void ScorePoint(int PlayerNumber)
    {
        _players[PlayerNumber - 1].Score += 1;
        CheckWinner();
    }

    private void CheckWinner()
    {
        foreach (Player p in _players)
        {
            if (p.Score >= _scoreToVictory)
            {
                ResetGame();
            }
        }
    }

    private void ResetGame()
    {
        foreach (Player p in _players)
        {
            p.Score = 0;
            
        }
        
    }
}
