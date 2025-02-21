using UnityEngine;

public class Draw : MonoBehaviour
{
    public Camera m_camera;
    public GameObject brush;
    public GameObject Canvas;
    [SerializeField] private float _Maxy;
    [SerializeField] private float _Miny;
    [SerializeField] private float _Maxx;
    [SerializeField] private float _Minx;

    LineRenderer currentLineRenderer;

    Vector3 lastPos;

    private void Update()
    {
        Drawing();
    }

    void Drawing() 
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBrush();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            PointToMousePos();
        }
        else 
        {
            currentLineRenderer = null;
        }
    }

    void CreateBrush() 
    {
        // Create the brush (line renderer object)
        GameObject brushInstance = Instantiate(brush);
        brushInstance.transform.SetParent(Canvas.transform);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        // Correctly calculate the mouse position in world space using ScreenToWorldPoint
        Vector3 mousePos = m_camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1160f));  // Correct z-depth for your scene
        mousePos.x = Mathf.Clamp(mousePos.x, _Minx, _Maxx);
        mousePos.y = Mathf.Clamp(mousePos.y, _Miny, _Maxy);
        // Set initial line positions (start and end at the same position)
        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);

        // Initialize lastPos to the current position
        lastPos = mousePos;
    }
    
    void AddAPoint(Vector3 pointPos) 
    {
        // Add a new point to the line renderer
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    void PointToMousePos() 
    {
        // Get the mouse position in world space using ScreenToWorldPoint
        Vector3 mousePos = m_camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1160f));
        mousePos.x = Mathf.Clamp(mousePos.x, _Minx, _Maxx);
        mousePos.y = Mathf.Clamp(mousePos.y, _Miny, _Maxy);
        // Only add a point if the mouse has moved
        if (lastPos != mousePos) 
        {
            AddAPoint(mousePos);  // Add the new point to the line renderer
            lastPos = mousePos;   // Update last position
        }
    }
}
