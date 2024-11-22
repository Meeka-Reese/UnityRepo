using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScores : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI HighScore;
    private float winScore; // Local variable for high score
    public GameBehavior GameBehavior;

    private void Start()
    {
        HighScore = GetComponent<TextMeshProUGUI>();
         
        UpdateHighScoreUI(); // Update the UI initially
    }

    private void Update()
    {
        // Ensure GameBehavior instance exists
        if (GameBehavior.Instance != null)
        {
            float currentScore = GameBehavior.Instance.HighScore;

            // Update winScore only if the current score is higher than the previous winScore
            if (currentScore > winScore)
            {
                winScore = currentScore; // Update winScore
            }
        }

        UpdateHighScoreUI(); // Update the UI with the current winScore
    }

    // Update the high score display
    private void UpdateHighScoreUI()
    {
        int minutes = Mathf.FloorToInt(winScore / 60.0f);
        int seconds = Mathf.FloorToInt(winScore % 60.0f);
        HighScore.text = $"Your Best Time Is {minutes:00}:{seconds:00}";
    }

    // Reset the high score manually
    public void ResetHighScore()
    {
        UpdateHighScoreUI(); // Update the UI to reflect the reset
    }
}