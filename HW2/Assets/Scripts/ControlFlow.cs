using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public bool flag;
    private int z = 2;
    private int y;
    // Start is called before the first frame update
    void Start()
    {


    if (flag)
    {
        Debug.Log("Boolean flag is set");
    }
    else
    {
        Debug.Log("Boolean flag is not set");
    }
    for (int x=0; x<10; x++)
    {
    y = (int)Mathf.Pow(z, x);
    Debug.Log($"The {x} power of 2 is {y}");
    }

}

    // Update is called once per frame
    void Update()
    {
        
    }
}
