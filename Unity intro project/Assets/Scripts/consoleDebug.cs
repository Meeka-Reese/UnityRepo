using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleDebug : MonoBehaviour
{
    public bool isSet;
    isSet = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello World");
        if (isSet)
        {
            Debug.Log("It's true");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
