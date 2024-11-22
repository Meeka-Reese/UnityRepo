using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    public float FollowSpeed = 10f;
    public Transform Target;
    [SerializeField] float YOffset = 3f;

    [SerializeField] private float MoveMult = .2f;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Aim = new Vector3(Target.position.x, Target.position.y + YOffset, Target.position.z);
        transform.position = Vector3.Slerp(transform.position, Aim * MoveMult, Time.deltaTime * FollowSpeed);
        
        
    }
}