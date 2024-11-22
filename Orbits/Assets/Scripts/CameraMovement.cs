using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private float _speed = 6.0f;

    private float deltaY;

    private float deltaX;

    private float deltaZ;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        deltaX = Input.GetAxis("Horizontal") * _speed;
        
            deltaZ = Input.GetAxis("Vertical") * _speed;


            if (Input.GetKey(KeyCode.Z))
            {
                deltaY = 2 * _speed;
            }
            else if (Input.GetKey(KeyCode.X))
            {
                deltaY = -2 * _speed;
            }
            else
            {
                deltaY = 0;
            }

            cam.transform.position += new Vector3(deltaX, deltaY, deltaZ);
        
    }
}
