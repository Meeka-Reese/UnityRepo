using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallBehavior : MonoBehaviour
{
    private float _speed;
    
    [SerializeField] float _yLimit = 4.0f;
    [SerializeField] float _xLimit = 10.0f;
    private AudioSource _source;

    private Vector2 _direction;
    [SerializeField] AudioClip _ballSound;
    [SerializeField] AudioClip _scoreSound;
    [SerializeField] AudioClip _wallSound;

    // Start is called before the first frame update
    void Start()
    {
        _speed = GameBehavior.Instance.InitBallSpeed;
        ResetBall();
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameBehavior.Instance.State == Utilities.GameplayState.Play)
        {
        transform.position += new Vector3(
            _speed * _direction.x,
            _speed * _direction.y
        ) * Time.deltaTime;
        if (Mathf.Abs(transform.position.y) >= _yLimit)
        {
            transform.position = new Vector3(
                transform.position.x, (_yLimit - 0.01f) * Mathf.Sign(transform.position.y), 0);
            _direction.y *= -1;
            _source.pitch = 1.0f;
            _source.PlayOneShot(_wallSound);
        }

        if (Mathf.Abs(transform.position.x) >= _xLimit)
        {
            GameBehavior.Instance.ScorePoint(
                transform.position.x > 0 ? 1 : 2
            );
            _source.pitch = 4f;
            _source.PlayOneShot(_scoreSound);
            ResetBall();
        }
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Paddle"))
        {
            _direction.x *= -1;
            _speed += GameBehavior.Instance.BallSpeedIncrement;
            _source.PlayOneShot(_ballSound);
            _source.pitch = 1.0f;
            
        }
        
    }

    void ResetBall()
    {
        transform.position = Vector3.zero;
        _direction = new Vector2(
            // condition ? passing : failing
            Random.value > 0.5f ? 1 : -1,
            Random.value > 0.5f ? 1 : -1
        );
        _speed = GameBehavior.Instance.InitBallSpeed;


    }
}
        

    

