using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;
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
    //Belongs to class not instance
    private void Start()
    {
        foreach (Player p in _players)
        {
            p.Score = 100;
        }
    }
}