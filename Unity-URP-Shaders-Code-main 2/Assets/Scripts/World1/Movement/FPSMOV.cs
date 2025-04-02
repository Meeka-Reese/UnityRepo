using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSMOV : MonoBehaviour
{
    [Range(1.0f, 2500.0f)]
    [SerializeField] private float _speed = 1600f;
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _rotationSpeed = 3f;
    [SerializeField] private float _keyRotationSpeed = 50f;
    [SerializeField] private float _SprintSpeed = 1.5f;

    [Header("Character Controller Settings")]
    [SerializeField] private float _slopeLimit = 45f;
    [SerializeField] private float _stepOffset = 0.5f;

    private CharacterController _controller;
    private Vector3 _moveDirection;
    private bool _isGrounded;
    private int Index = 0;

    private AudioSource _currentAudioSource;
    private AudioSource _nextAudioSource;
    [SerializeField] private AudioClip[] WalkSteps;

    private bool isCrossfading = false;

    private void Start()
    {
        // Create two AudioSources for crossfading
        _currentAudioSource = gameObject.AddComponent<AudioSource>();
        _nextAudioSource = gameObject.AddComponent<AudioSource>();

        SetupAudioSource(_currentAudioSource);
        SetupAudioSource(_nextAudioSource);

        _controller = GetComponent<CharacterController>();
        _controller.slopeLimit = _slopeLimit;
        _controller.stepOffset = _stepOffset;
    }

    private void SetupAudioSource(AudioSource source)
    {
        source.playOnAwake = false;
        source.loop = true; // Set to loop since footstep sounds repeat
        source.volume = 0f; // Start with 0 volume
    }

    private void Update()
    {
        _isGrounded = _controller.isGrounded;
        float MoveMult;
        if (Input.GetKey(KeyCode.LeftShift | KeyCode.RightShift))
        {
            MoveMult = _SprintSpeed;
        }
        else
        {
            MoveMult = 1f;
        }
        // Get input for movement
        float deltaX = Input.GetAxis("Horizontal") * _speed * MoveMult * Time.deltaTime;
        float deltaZ = Input.GetAxis("Vertical") * _speed * MoveMult* Time.deltaTime;

        // Calculate movement vector
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = transform.TransformDirection(movement);

        // Apply gravity
        if (_isGrounded)
        {
            _moveDirection.y = -1f;
        }
        else
        {
            _moveDirection.y += _gravity * Time.deltaTime;
        }

        _moveDirection.x = movement.x;
        _moveDirection.z = movement.z;

        // Move the character
        _controller.Move(_moveDirection);

      
        if (Input.GetKey(KeyCode.X))
        {
            transform.Rotate(0, _keyRotationSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            transform.Rotate(0, -_keyRotationSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.C))
        {
            transform.Rotate(_keyRotationSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.V))
        {
            transform.Rotate(-_keyRotationSpeed * Time.deltaTime, 0, 0);
        }


        // Keep rotation constrained to the Y-axis
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

        // Handle audio crossfade logic
        if (Mathf.Abs(deltaX) > 0 || Mathf.Abs(deltaZ) > 0)
        {
            // Movement detected
            HandleCrossfade(true);
        }
        else
        {
            // No movement
            HandleCrossfade(false);
        }
    }

    private void HandleCrossfade(bool isMoving)
    {
        if (isMoving)
        {
            // If the clip needs to change, start a crossfade
            if (_currentAudioSource.clip != WalkSteps[Index] && !isCrossfading)
            {
                _nextAudioSource.clip = WalkSteps[Index];
                _nextAudioSource.Play();
                StartCoroutine(CrossfadeAudio(_currentAudioSource, _nextAudioSource));
            }

            // If already playing the correct clip, fade in or resume
            if (!isCrossfading)
            {
                if (!_currentAudioSource.isPlaying)
                {
                    _currentAudioSource.clip = WalkSteps[Index];
                    _currentAudioSource.Play();
                }

                _currentAudioSource.volume = Mathf.MoveTowards(_currentAudioSource.volume, .7f, Time.deltaTime * 2f);
            }
        }
        else
        {
            // Fade out the current audio if no movement
            if (_currentAudioSource.isPlaying && _currentAudioSource.volume > 0f)
            {
                _currentAudioSource.volume = Mathf.MoveTowards(_currentAudioSource.volume, 0f, Time.deltaTime * 2f);

                // Stop the audio once volume is fully faded out
                if (_currentAudioSource.volume <= 0f)
                {
                    _currentAudioSource.Stop();
                }
            }
        }
    }


    private IEnumerator CrossfadeAudio(AudioSource fromSource, AudioSource toSource, float fadeDuration = 0.5f)
    {
        isCrossfading = true;
        float timer = 0f;

        // Gradually fade out the current source and fade in the next source
        while (timer < fadeDuration)
        {
            float progress = timer / fadeDuration;
            fromSource.volume = Mathf.Lerp(1f, 0f, progress);
            toSource.volume = Mathf.Lerp(0f, 1f, progress);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure final states
        fromSource.volume = 0f;
        toSource.volume = 1f;

        // Swap the sources
        AudioSource temp = _currentAudioSource;
        _currentAudioSource = _nextAudioSource;
        _nextAudioSource = temp;

        fromSource.Stop(); // Stop the old source
        isCrossfading = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Grass"))
        {
            Index = 0;
        }
        else if (hit.gameObject.CompareTag("Wood"))
        {
            Index = 1;
        }
        else if (hit.gameObject.CompareTag("Dirt"))
        {
            Index = 2;
        }
        else if (hit.gameObject.CompareTag("Sand"))
        {
            Index = 3;
        }
    }
}
