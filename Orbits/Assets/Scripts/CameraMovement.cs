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
   
    public Transform target; 
    public float radius = 5000f; 
    public float speed = 3f; 
    public Vector3 offset = Vector3.up; 

    private float angle = 0f; 

  
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
                radius += deltaY;
            }
            else if (Input.GetKey(KeyCode.X))
            {
                deltaY = -2 * _speed;
                radius += deltaY;
            }
            else
            {
                deltaY = 0;
            }
            

            angle += speed * Time.deltaTime;
            angle %= 360f; 
            
       
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        
            
            Vector3 newPosition = new Vector3(x, target.position.y + offset.y, z) + target.position;
            transform.position = newPosition;
            transform.LookAt(target.position + offset);
        
    }
}
