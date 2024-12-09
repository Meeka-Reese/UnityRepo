using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; 
    public float radius = 5000f; 
    public float speed = 3f; 
    public Vector3 offset = Vector3.up; 

    private float angle = 0f; 

    void Update()
    {
        

        
        angle += speed * Time.deltaTime;
        angle %= 360f; 
       
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        
        Vector3 newPosition = new Vector3(x, offset.y, z) + target.position;
        transform.position = newPosition;
        transform.LookAt(target.position + offset);
    }
}