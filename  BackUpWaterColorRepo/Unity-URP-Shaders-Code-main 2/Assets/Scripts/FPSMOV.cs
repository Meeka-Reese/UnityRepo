using System;
using UnityEngine;

// Require a CharacterController component on the object.
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]

public class FPSMOV : MonoBehaviour
{
    [Range(1.0f, 1600.0f)]
    [SerializeField] private float _speed = 1600f;
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _rotationSpeed = 3f;
    [SerializeField] private float _keyRotationSpeed = 50f;

    [Header("Character Controller Settings")]
    [SerializeField] private float _slopeLimit = 45f; // Maximum slope angle (in degrees)
    [SerializeField] private float _stepOffset = 0.5f; // Height the character can step over

    private CharacterController _controller;
    private Vector3 _moveDirection;
    private bool _isGrounded;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _controller.slopeLimit = _slopeLimit;
        _controller.stepOffset = _stepOffset;
    }

    private void Update()
    {
        _isGrounded = _controller.isGrounded; // Check if the character is on the ground

        // Get input for movement
        float deltaX = Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
        float deltaZ = Input.GetAxis("Vertical") * _speed * Time.deltaTime;

        // Calculate movement vector relative to the player's forward direction
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = transform.TransformDirection(movement);

        // Apply gravity only when the character is not grounded
        if (_isGrounded)
        {
            _moveDirection.y = -1f; // A small downward force to keep the character grounded
        }
        else
        {
            _moveDirection.y += _gravity * Time.deltaTime; // Apply gravity when airborne
        }

        // Combine movement with vertical direction (gravity or slope adjustment)
        _moveDirection.x = movement.x;
        _moveDirection.z = movement.z;

        // Move the character
        _controller.Move(_moveDirection);

        // Handle rotation using key input
        if (Input.GetKey(KeyCode.X))
        {
            transform.Rotate(0, _keyRotationSpeed, 0 * Time.deltaTime); // Rotate negatively
        }
        if (Input.GetKey(KeyCode.Z))
        {
            transform.Rotate(0, -_keyRotationSpeed, 0 * Time.deltaTime); // Rotate positively
        }

        // Keep rotation constrained to the Y-axis
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
