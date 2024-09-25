using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBehavior : MonoBehaviour
{
    public float Speed = 5.0f;

    public KeyCode UpKey;
    public KeyCode DownKey;

    public float yLim = 3.8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(UpKey) && transform.position.y < yLim)
        {
            transform.position += new Vector3(0, Speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(DownKey) && transform.position.y > -yLim)
        {
            transform.position += new Vector3(0, -Speed * Time.deltaTime, 0);
        }

    }
}
