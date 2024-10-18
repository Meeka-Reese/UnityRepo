using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleHighScore : MonoBehaviour
{
    // [SerializeField] private float _duration;
    [SerializeField] private TextMeshProUGUI CurrentHighScore;
    private float WinScore;
    public TitleSceneManager TitleSceneManager;
   

    
    //Next step assign high score time to variable in title scene manager and then use that to display the time in
    //this script.

    

    private void Start()
    {
        
        CurrentHighScore = GetComponent<TextMeshProUGUI>();
    }
    
    void Update()
    {
        
        int minutes = Mathf.FloorToInt(WinScore / 60.0f);
        int seconds =Mathf.FloorToInt(WinScore % 60.0f);
        CurrentHighScore.text = $"Your Best Time Is {minutes:00}:{seconds:00}";
        WinScore = GameBehavior.Instance.HighScore;

    }
}