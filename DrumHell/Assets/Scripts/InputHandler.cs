using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InputHandler : MonoBehaviour
{
    private Camera mainCamera;
    
    



    private void Awake()
    {
        mainCamera = Camera.main;
        

    }

    public void OnClick(InputAction.CallbackContext context)
    {
        // if (!context.started) return;
        // var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        // if (!rayHit.collider) return;
        // Debug.Log(rayHit.collider.gameObject.name);
        // if (rayHit.collider.tag == "Home")
        // {
        //     StopAllCoroutines();
        //     Debug.Log("Click");
        //     SceneManager.LoadScene("Title");
        // }
    }
}
