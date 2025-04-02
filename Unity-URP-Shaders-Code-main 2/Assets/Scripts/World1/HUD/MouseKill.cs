using UnityEngine;

public class MouseKill : MonoBehaviour
{
    [SerializeField] private MouseDragScript _mouseDragScript;
    [SerializeField] private float distance = 100f;
    [SerializeField] private float xBound = 5000f;
    [SerializeField] private float yBound = 5000f;
    [SerializeField] private float Multiplier = 0.1425f;

    private GameObject MouseCollider;
    private bool IsMouseWithinBounds = false;
    private bool IsMinimized = false;

    private Vector3 MinBounds;
    private Vector3 MaxBounds;
    private Camera MainCamera;

    private void Start()
    {
        MainCamera = Camera.main;
        MouseCollider = GameObject.Find("MouseCollider");
    }

    private void Update()
    {
        // Calculate world-space bounds
        Vector3 WorldMinBounds = CalculatePosition(-xBound, -yBound);
        Vector3 WorldMaxBounds = CalculatePosition(xBound, yBound);

        // Convert to screen space
        MinBounds = MainCamera.WorldToScreenPoint(WorldMinBounds);
        MaxBounds = MainCamera.WorldToScreenPoint(WorldMaxBounds);

        // Check if the mouse is within bounds
        IsMouseWithinBounds = Input.mousePosition.x > MinBounds.x && Input.mousePosition.x < MaxBounds.x &&
                              Input.mousePosition.y > MinBounds.y && Input.mousePosition.y < MaxBounds.y;

        // Update collider state based on bounds and minimize state
        IsMinimized = _mouseDragScript._minimize;
        MouseCollider.SetActive(!IsMinimized && IsMouseWithinBounds);

        Debug.Log($"Mouse Within Bounds: {IsMouseWithinBounds}, Minimized: {IsMinimized}");
    }

    private Vector3 CalculatePosition(float x, float y)
    {
        Vector3 forwardPosition = MainCamera.transform.position + MainCamera.transform.forward * distance;

        Vector3 xOffset = Vector3.Cross(MainCamera.transform.forward, Vector3.up).normalized * (x - transform.position.x) * Multiplier;
        Vector3 yOffset = Vector3.Cross(MainCamera.transform.forward, Vector3.right).normalized * (y - transform.position.y) * Multiplier;

        return forwardPosition + xOffset + yOffset;
    }
}
