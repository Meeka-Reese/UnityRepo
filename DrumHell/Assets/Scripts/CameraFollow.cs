using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 10f;
    public Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Aim = new Vector3(Target.position.x, transform.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, Aim, Time.deltaTime * FollowSpeed);
        
        
    }
}
