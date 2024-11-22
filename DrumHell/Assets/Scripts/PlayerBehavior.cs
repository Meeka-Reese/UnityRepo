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
    public Utilities.PauseState pauseState;

    public PlatBehav PlatBehav;
    public Animator _animator;
    public Animator _animator2;
    private bool runf = false;
    private bool runb = false;

    [SerializeField] float LoudnessMult = 1000f;
    [SerializeField] float Loudness;
    private float yPos;
    private float xPos;
    [SerializeField] private float TelOffset;
    public LoudnessMonitor LoudnessMonitor;
    public PlatBehav _platBehav;
    
    private float MoveSize;
    public bool BallDead = false;
    [SerializeField] float yLim = -20f;
    [SerializeField] float _speed = 6.0f;
    private Rigidbody2D rb;
    private float horizontal;
    private bool isFacingRight = true;
    private bool dJumpUsed = false;
    private float JumpIndex = 0;
    private bool DbJump = false;
    private bool Roll = false;
    private bool IsPlaying = false;
    public bool win  = false;
    [SerializeField] private float maxJumpVelocity = 10f;
    [SerializeField] private BoxCollider2D BoxCollider;
    [SerializeField] private CircleCollider2D CircleCollider;
    [SerializeField] private float JumpPulse;
    private bool slantCollison = false;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _floorHit;
    [SerializeField] private AudioClip _powerUp;
    [SerializeField] private AudioClip _powerDown;
    [SerializeField] private AudioClip _walkLoop;
    
    
    
    
    private float SpeedBoost;
   
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        
        
        SpeedBoost = 0f;
        BoxCollider = GetComponent<BoxCollider2D>();
        CircleCollider = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        _animator2 = GameObject.Find("DoubleJump").GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        BoxCollider.enabled = true;
        CircleCollider.enabled = false;
        _source = GetComponent<AudioSource>();
        

    }
    

    // Update is called once per frame
    void Update()
    {
        
        transform.rotation = Quaternion.identity;
        horizontal = Input.GetAxisRaw("Horizontal");
        
        
        _animator.SetBool("Runf", runf);
        _animator.SetBool("Runb", runb);
        _animator.SetBool("Roll", Roll);
        _animator2.SetBool("DbJump", DbJump);
        
        
        Loudness = PlatBehav.Loudness;
        MoveSize = _platBehav.MoveSize;
        if (transform.position.y < yLim)
        {
            BallDead = true;
        }
        
        
        Roll = _rigidbody.velocity.y < -10 ? true : false;
        if (Roll)
        {
            BoxCollider.enabled = false;
            CircleCollider.enabled = true;
        }
        else if (!Roll && !slantCollison)
        {
            BoxCollider.enabled = true;
            CircleCollider.enabled = false;
        }


        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (JumpIndex)
            {
                case 0:
                    JumpIndex++;
                    JumpVelocity();
                    break;
                case 1:
                    JumpIndex++;
                    DbJump = true;
                    JumpVelocity();
                    break;
                case 2:
                    if (groundState == Utilities.GroundState.Ground)
                        JumpIndex = 0;
                    break;
        }
           

            }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            
            runf = true;
            runb = false;
            
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            runf = false;
            runb = true;
            
        }
        else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            runf = false;
            runb = false;
            
        }
        // if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) && groundState == Utilities.GroundState.Ground) {
        //     if (!_source.isPlaying) {
        //         _source.clip = _walkLoop;
        //         _source.loop = true;  
        //         _source.Play();
        //     }
        // } 
        // else 
        // {
        //     _source.loop = false;
        // }
    }

    void JumpVelocity()
    {
        
            float CurrentYVelocity = _rigidbody.velocity.y;
            float ClampedYVelocity = Mathf.Clamp(CurrentYVelocity + _jumps, -Mathf.Infinity, maxJumpVelocity);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, ClampedYVelocity);
            groundState = Utilities.GroundState.Air;
            runf = false;
            runb = false;
            _source.PlayOneShot(_jumpSound);
        



    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * _speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("SoundPlat") || other.gameObject.CompareTag("Slant"))
        {
            groundState = Utilities.GroundState.Ground;
            DbJump = false;
            JumpIndex = 0;
            if (!IsPlaying)
                StartCoroutine(DelayPlay());
            
            
        }

        if (other.gameObject.CompareTag("FinishPlat"))
        {
            win = true;
        }
        if (other.gameObject.CompareTag("Powerdown"))
        {
            _source.PlayOneShot(_powerDown);
        }

        if (other.gameObject.CompareTag("Powerup"))
        {
            _source.PlayOneShot(_powerUp);
        }
        if (other.gameObject.CompareTag("crusher"))
        {
            BallDead = true;
        }


    }

    private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("SoundPlat"))
            {
                
                SpeedBoost = 5 * Loudness;
                transform.SetParent(other.transform);
                
               
                Debug.Log(Loudness);
               
            
            }
            if (other.gameObject.CompareTag("Slant"))
            {
                groundState = Utilities.GroundState.Ground;
                DbJump = false;
                JumpIndex = 0;
                Roll = true;
                Vector2 velocity = _rigidbody.velocity;
                velocity.y += -11;
                _rigidbody.velocity = velocity;      
                slantCollison = true;
                


            }
            
        }

    

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("SoundPlat"))
        {
            StartCoroutine(ExitDelay());
        }
        transform.SetParent(null);
        if (other.gameObject.CompareTag("Slant"))
        {
            slantCollison = false;
        }
        
    }

    IEnumerator ExitDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SpeedBoost = 0f;
        
    }

    IEnumerator DelayPlay()
    {
        _source.PlayOneShot(_floorHit);
        IsPlaying = true;
        yield return new WaitForSeconds(0.3f);
        IsPlaying = false;
    }

    }

