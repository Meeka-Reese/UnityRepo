using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPlat : MonoBehaviour
{
public bool Win = false;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Win = true;
        }
    }
}
