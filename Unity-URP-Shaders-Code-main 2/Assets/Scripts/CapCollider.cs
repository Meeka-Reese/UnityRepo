using UnityEngine;

public class CapCollider : MonoBehaviour
{
    public bool Collision = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision collision)
    {
        Collision = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        Collision = false;
    }
}
