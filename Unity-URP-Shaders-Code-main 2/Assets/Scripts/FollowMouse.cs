using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float distance = 20f;

    void Start()
    {
        // Automatically find the main camera if not assigned
        cam = GameObject.Find("SecondCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (cam == null) return;

        // Get the mouse position in screen space
        Vector3 screenMousePos = Input.mousePosition;

        // Set the Z position relative to the camera's forward direction
        screenMousePos.z = distance;

        // Convert the screen position to world position
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(screenMousePos);

        // Update the object's position to follow the mouse while maintaining the distance
        transform.position = mouseWorldPos;
    }
}