using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float distance = 20f;
    [SerializeField] private float CursorSpeed = 5f;
    private GameObject MouseColliderSlow;
    private GameObject MouseColliderSlower;

    void Start()
    {
        // Automatically find the main camera if not assigned
        cam = GameObject.Find("SecondCamera").GetComponent<Camera>();
        MouseColliderSlow = GameObject.Find("MouseColliderSlow");
        MouseColliderSlower = GameObject.Find("MouseColliderSlower");
    }

    void Update()
    {
        float time = Time.deltaTime * CursorSpeed;
        float slowtime = Time.deltaTime * (CursorSpeed-1);
        float slowertime = Time.deltaTime * (CursorSpeed-2);
        if (cam == null) return;

        // Get the mouse position in screen space
        Vector3 screenMousePos = Input.mousePosition;

        // Set the Z position relative to the camera's forward direction
        screenMousePos.z = distance;

        // Convert the screen position to world position
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(screenMousePos);

        // Update the object's position to follow the mouse while maintaining the distance
        transform.position = Vector3.Lerp(transform.position, mouseWorldPos, time);
        MouseColliderSlow.transform.position = Vector3.Lerp(MouseColliderSlow.transform.position, transform.position, slowtime);
        MouseColliderSlower.transform.position = Vector3.Lerp(MouseColliderSlower.transform.position, transform.position, slowertime);
    }
}