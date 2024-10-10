using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BallMov : MonoBehaviour
{
    public float Speed = 3.0f;
    public float YLimit = 3.9f;
    public float XLimit = 6.3f;
    [SerializeField] AudioClip BouncePaddle;
    [SerializeField] AudioClip BounceWall;
    [SerializeField] AudioClip DeathSound;
    [SerializeField] AudioClip ScoreSound;
    private AudioSource _source;
    [SerializeField] float xDistance;
    [SerializeField] float yDistance;
    [SerializeField] private float xPaddleDistance;
    
    [SerializeField] private float OverallBallSpeed;
    [SerializeField] private float RSpeed = 5.0f;
    
    
    

    private Vector2 _direction;
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _direction = new Vector2(
            // condition ? passing : failing
            Random.value > 0.5f ? 1 : -1, 
            -1
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (GameBehavior.Instance.State == Utilities.GameplayState.Play)
        {
            transform.position += new Vector3(
                Speed * _direction.x,
                Speed * _direction.y
            ) * Time.deltaTime;
            if ((transform.position.y) >= YLimit)
            {
                _direction.y *= -1;
                
                
                _source.PlayOneShot(BounceWall);
                
            }
            else if ((transform.position.y) <= -YLimit)
            {
                ResetBall();
            }

            if (Mathf.Abs(transform.position.x) >= XLimit)
            {
                _direction.x *= -1;
                _source.PlayOneShot(BounceWall);
            }
        }




    }
    private void ResetBall()
    {
        _source.PlayOneShot(DeathSound);
        GameBehavior.Instance.AddScore = 0;
        // Destroy(this.gameObject);
        GameBehavior.Instance.ResetGame = true;
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Brick"))
        {
            _direction.x *= -1; 
            _source.PlayOneShot(ScoreSound);
            GameBehavior.Instance.AddScore += 1;
            xDistance = transform.position.x - other.transform.position.x;
            yDistance = transform.position.y - other.transform.position.y;
            // _direction.x = xDistance >= 0 ? _direction.x: -_direction.x;
            _direction.y = yDistance >= 0 ? _direction.y: -_direction.y;

        }
        else if (other.transform.CompareTag("Paddle"))
        {
            
            xPaddleDistance = transform.position.x - other.transform.position.x;
            _direction.x = (_direction.x += Mathf.Abs(xPaddleDistance));
            _direction.x = xPaddleDistance < 0 ? -_direction.x : _direction.x;
            OverallBallSpeed = Mathf.Abs(_direction.y) + Mathf.Abs(_direction.x);
            _direction.x += _direction.x <= 0 ? (OverallBallSpeed - RSpeed) / 2 : -((OverallBallSpeed - RSpeed) / 2);
            _direction.y += _direction.y <= 0 ? (OverallBallSpeed - RSpeed) / 2 : -((OverallBallSpeed - RSpeed) / 2);
            
            _direction.y *= -1;
            
            
            
            _source.PlayOneShot(BouncePaddle);
        }
        
    }
}