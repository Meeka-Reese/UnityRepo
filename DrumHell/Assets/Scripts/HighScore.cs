using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScores : MonoBehaviour
{
    // [SerializeField] private float _duration;
    [SerializeField] private TextMeshProUGUI HighScore;
    private float WinScore;
    public GameBehavior GameBehavior;
    

    public float ElapsedTime
    {
        get => WinScore;
        set
        {
            // WinScore = value;
            // int minutes = Mathf.FloorToInt(WinScore / 60.0f);
            // int seconds =Mathf.FloorToInt(WinScore % 60.0f);

            
        }
    }

    private void Start()
    {
        
        HighScore = GetComponent<TextMeshProUGUI>();
    }
    
    void Update()
    {
        int minutes = Mathf.FloorToInt(WinScore / 60.0f);
        int seconds =Mathf.FloorToInt(WinScore % 60.0f);
        HighScore.text = $"Your Best Time Is {minutes:00}:{seconds:00}";
        WinScore = GameBehavior.Instance.HighScore;

    }
}