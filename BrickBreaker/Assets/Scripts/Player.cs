using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int _score = 0;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public int Score
    {
        get => _score;
        
        set
        {
            // update value of backing variable
            _score = value;
            // use getter property to update the GUI
            _scoreText.text = Score.ToString();
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Ball"))
        {
            _score++;
            
        }
       
    }
}