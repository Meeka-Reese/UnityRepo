using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{
    [SerializeField] private float _duration;
    private TextMeshProUGUI _clockGui;
    public float _elapsedTime;
    public PlayerBehavior _player;
    public bool Win = false;
    private bool CoroutineRunning = false;
    public float ElapsedTime
    {
        get => _elapsedTime;
        set
        {
            _elapsedTime = value;
            int minutes = Mathf.FloorToInt(_elapsedTime / 60.0f);
            int seconds =Mathf.FloorToInt(_elapsedTime % 60.0f);
            
            _clockGui.text = $"{minutes:00}:{seconds:00}";
        }
    }

    private void Start()
    {
        _clockGui = GetComponent<TextMeshProUGUI>();
        _player = GetComponent<PlayerBehavior>();
        if (_player == null)
        {
            _player = GameObject.FindObjectOfType<PlayerBehavior>(); // Finds the PlayerBehavior in the scene
            if (_player == null)
            {
                Debug.LogError("PlayerBehavior component not found!");
                return;
            }
        }
        
        

    }
    
    void Update()
    {
        
        if (_player == null)
            Start();
        Win = _player.win;
        
        if (!Win)
        {
            ElapsedTime += Time.deltaTime;
        }

        if (Win && !CoroutineRunning)
        {
            StartCoroutine(Restart());
        }

        if (_elapsedTime >= _duration)
        {
            
            ElapsedTime -= _duration;
        }

        if (_player.BallDead)
        {
            ElapsedTime = 0;

        }
    }
    

    IEnumerator Restart()
    {
        CoroutineRunning = true;
        yield return new WaitForSeconds(1f);
        ElapsedTime = 0f;
        SceneManager.LoadScene("Title");
        CoroutineRunning = false;
        Win = false;
    }
}