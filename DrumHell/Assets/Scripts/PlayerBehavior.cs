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
    private float yPos;
    private float xPos;
    [SerializeField] private float TelOffset;
    public LoudnessMonitor LoudnessMonitor;
    public PlatBehav _platBehav;
    private AudioSource Source;
    private float MoveSize;
    
    private float SpeedBoost;
   
    
    
    // Start is called before the first frame update
    void Start()
    {
        Source = PlatBehav._source;
        
        SpeedBoost = 0f;




    }

    // Update is called once per frame
    void Update()
    {
        Loudness = PlatBehav.Loudness;
        MoveSize = _platBehav.MoveSize;
        
        
        
        if (Input.GetKeyDown(KeyCode.Space) && groundState == Utilities.GroundState.Ground)
        {
            _rigidbody.AddForce(new Vector2(0, _jumps), ForceMode2D.Impulse);
            groundState = Utilities.GroundState.Air;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3((_runspeed + SpeedBoost) * Time.deltaTime, 0, 0);
            
            
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3((-_runspeed + -SpeedBoost) * Time.deltaTime, 0, 0);
            
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("SoundPlat"))
        {
            groundState = Utilities.GroundState.Ground;
        }


    }

    private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("SoundPlat"))
            {
                Vector3 PMax = new Vector3(_platBehav.xMax, transform.position.y, transform.position.z);
                Vector3 PLim = new Vector3(_platBehav.xLim, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(PLim, PMax, (Loudness)* MoveSize);
                SpeedBoost = 5 * Loudness;
               
                Debug.Log(Loudness);
               
            
            }
            
        }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("SoundPlat"))
        {
            StartCoroutine(ExitDelay());
        }
        
    }

    IEnumerator ExitDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SpeedBoost = 0f;
        
    }

    }

