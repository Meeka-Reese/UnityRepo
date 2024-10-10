using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour

{
    [SerializeField] float _jumps = 1f;
    [SerializeField] float _runspeed = 3f;
    [SerializeField] Rigidbody2D _rigidbody;
    public Utilities.GroundState groundState;

    public PlatBehav PlatBehav;

    [SerializeField] float LoudnessMult = 1000f;
    [SerializeField] float Loudness;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Loudness = PlatBehav.Loudness;
        
        if (Input.GetKeyDown(KeyCode.Space) && groundState == Utilities.GroundState.Ground)
        {
            _rigidbody.AddForce(new Vector2(0, _jumps), ForceMode2D.Impulse);
            groundState = Utilities.GroundState.Air;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(_runspeed * Time.deltaTime, 0, 0);
            
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-_runspeed * Time.deltaTime, 0, 0);
            
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("SoundPlat"))
        {
            groundState = Utilities.GroundState.Ground;
        }

        if (other.gameObject.CompareTag("SoundPlat"))
        {
            Debug.Log(Loudness);
            _rigidbody.AddForce(new Vector2(0, Loudness * LoudnessMult), ForceMode2D.Impulse);
            
        }

    }
}
