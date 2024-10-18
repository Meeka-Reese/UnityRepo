using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    private void Start()
    {
        
    }

    private void Update()
    {
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
                SceneManager.LoadScene("Title");
            
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

  
