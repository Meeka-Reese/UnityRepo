using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBehavior : MonoBehaviour
{
    
    [SerializeField] float _speed = 5.0f;
    [SerializeField] KeyCode UpKey;
    [SerializeField] KeyCode DownKey;
    [SerializeField] float _yLim = 3.8f;
    // SerializedField modifiable by inspecter but not by other classes
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameBehavior.Instance.State == Utilities.GameplayState.Play)
        {
            if (Input.GetKey(UpKey) && transform.position.y < _yLim)
            {
                transform.position += new Vector3(0, _speed * Time.deltaTime, 0);
            }

            if (Input.GetKey(DownKey) && transform.position.y > -_yLim)
            {
                transform.position += new Vector3(0, -_speed * Time.deltaTime, 0);
            }
        }

    }
}
