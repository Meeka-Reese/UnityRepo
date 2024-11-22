using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyGameObject : MonoBehaviour
{
    public static DontDestroyGameObject Instance;
    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            //destroy duplicates
            Destroy(gameObject);
        }
        else
        {
            //Runs first time around
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // Uncomment on level based games
            // DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    

   
}