using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMov : MonoBehaviour
{
    public float Speed = 5.0f;

    public KeyCode RightKey;
    public KeyCode LeftKey;
    private float XLim = 5.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(RightKey) && transform.position.x < XLim)
        {
            transform.position += new Vector3(Speed *Time.deltaTime, 0, 0);
            
        }
        if(Input.GetKey(LeftKey) && transform.position.x > -XLim)
        {
            transform.position += new Vector3(-Speed *Time.deltaTime, 0, 0);
            
        }
        
    }
}
