using UnityEngine;

public class NotificationSpin : MonoBehaviour
{
    [SerializeField] private float _spinSpeed = 10f;


    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * _spinSpeed);
    }
}
