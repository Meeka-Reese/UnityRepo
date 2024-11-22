using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;
    public WinPlat WinPlat;
    public bool win = false;
    public Clock clock;
    public float timer;
    public float EndTime;
    public float HighScore = 0;
    [SerializeField] Player[] _players = new Player[1];
    public PlayerBehavior PlayerBehavior;
    public bool Loose = false;
    public Utilities.PauseState pauseState;
    public bool play = true;
    [SerializeField] private GameObject pauseButton;
    
    
   
    private void Awake()
    {
       pauseButton = GameObject.Find("PauseButton");
       if (pauseButton == null)
       {
           Debug.LogError("PauseButton not found");
       }
       pauseButton.SetActive(false);
        pauseState = Utilities.PauseState.Play;
        
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

    private void Start()
    {
        
    }

    private void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            
            pauseState = pauseState == Utilities.PauseState.Pause ? Utilities.PauseState.Play : Utilities.PauseState.Pause;
            if (pauseState == Utilities.PauseState.Pause)
            {
                Time.timeScale = 0;
                play = false;
                pauseButton.SetActive(true);
            }
            else if (pauseState == Utilities.PauseState.Play)
            {
                Time.timeScale = 1;
                
                play = true;
                pauseButton.SetActive(false);
                
            }
            Debug.Log(pauseState);
        }
        
        Loose = PlayerBehavior.BallDead;
        if (Loose == true)
        {
            SceneManager.LoadScene("Title");
        }
        timer = clock.ElapsedTime;
        win = WinPlat.Win;
        if (win)
        {
            EndTime = timer;
            if (HighScore == 0)
            {
                HighScore = EndTime;
            }
            else
            {
                HighScore = EndTime >= HighScore ? HighScore : EndTime;
            }
            if (win)
            {
                // SceneManager.LoadScene("Title");
            
            }
            



        }
    }

    private void HighScoreSet()
    {
        foreach (Player p in _players)
        {
            p.Score = Mathf.FloorToInt(HighScore);
            
        }
        
    }
}

  
