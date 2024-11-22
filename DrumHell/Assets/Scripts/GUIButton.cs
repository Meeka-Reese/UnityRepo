using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIButton : MonoBehaviour
{
    private GameBehavior gameBehavior;
    private bool clicked = false;
    
    // Start is called before the first frame update
    public void HomeClick()
    {
        Debug.Log("Home clicked");
        SceneManager.LoadScene("Title");
    }

    public void QuitClick()
    {
        Debug.Log("quit clicked");
        Application.Quit();
    }

    public void PauseClick()
    {
        Debug.Log("Pause Clicked");
        clicked = true;
    }

    public void Update()
    {
        clicked = gameBehavior.play;
    }
}
