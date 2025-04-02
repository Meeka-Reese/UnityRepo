using System.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseDragScript : MonoBehaviour
{
    [SerializeField] private GameObject target;
    
    [SerializeField] private GameObject gridHolder;
    private GameObject Tab;
    private float yMin;
    private float yMax;
    private float xMin;
    private float xMax;
    private float journeyLength;
    private float startTime;
    private Vector3 MinimizePos;
    private Vector3 lastVector3;
    private Vector3 lastScaleTarget;
    private Vector3 lastScalePaper;
    private Vector3 lastScaleGrid;
    private Vector3 Dragpos;
    private Vector3 screenPos;

    [Header("Values For Tweaking Drag")] 
    [SerializeField] private float adjustedYOffset;
    [SerializeField] private float  Multiplier, XGridOff, YGridOff, ScreenHieghtBounds, ScreenWidthBounds;
    [Header("RaycastAdjustment")]
    [SerializeField] private Vector2 raycastOffset;
    [Header("Everything Else")]
    
    
    [SerializeField] private float distance = 20f;

    private float adjustedY;
    private float xBoundMin;
    private float xBoundMax;
    private float yBoundMin;
    private float yBoundMax;
    private bool _maximize;
    private Camera mainCamera;
    public bool isDragging;
    
    
    [SerializeField] private Material Outlines;
    private Vector3 TargetPos;
    
    private Vector3 TargetPosPaper;
    private Vector3 TargetPosGrid;
    private Vector3 InitialPosGrid;
    private Vector3 InitialPosPaper;
   
    [SerializeField] private float TransitionSpeed = .55f;
    public float maskScale;
    public bool _minimize;
    private Vector3 minimizeStartPos;
    private Vector3 minimizeTargetPos;
    private Vector3 targetScaleTarget = Vector3.zero;
    private Vector3 targetScalePaper = Vector3.zero;
    private Vector3 targetScaleGrid = Vector3.zero;
    
    private Vector3 initialPaperPos;
    private Vector3 initialGridPos;
    private float transitionProgress = 0f;
    private Vector3 initialScaleTarget;
    private Vector3 initialScalePaper;
    private Vector3 initialScaleGrid;
    [SerializeField] private GameObject RayCastTarget;
    [SerializeField] private GameObject SoundRayCastTarget;
    [SerializeField] private GameObject MouseCollider;
    
    private Camera cam;
    private Camera cam2;
    
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    private bool Initial = true;

    private void Start()
    {
        
        cam2 = GameObject.Find("SecondCamera").GetComponent<Camera>();
        RayCastTarget.SetActive(true);
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
        cam = Camera.main;
       _maximize = false;
        _minimize = false;
        mainCamera = Camera.main;
        gridHolder = GameObject.Find("Grid Holder");
       
        target = GameObject.Find("PaintTab");
        Tab = GameObject.Find("Tab");
        Tab.SetActive(false);
       
        InitialPosGrid = gridHolder.transform.position;
        minimizeStartPos = target.transform.position;

        initialGridPos = gridHolder.transform.position;
        initialScaleTarget = target.transform.localScale;
       
        initialScaleGrid = gridHolder.transform.localScale;
        lastScaleTarget = target.transform.localScale;

        lastScaleGrid = gridHolder.transform.localScale;

        // Initialize TargetPos to the target's starting position
        TargetPos = target.transform.position;
        Outlines.SetFloat("MaskKill", 1f);
        gridHolder.transform.position = (cam2.transform.position + cam2.transform.forward * distance);
        gridHolder.transform.eulerAngles = new Vector3(0, 0 , 0);

        StartCoroutine(StartJult());

    }

    private void Update()
    {
        
       
        // gridHolder.transform.rotation = Quaternion.Euler(
        //     mainCamera.transform.eulerAngles.x,
        //     mainCamera.transform.eulerAngles.y,
        //     mainCamera.transform.eulerAngles.z);
       
        // gridHolder.transform.rotation = mainCamera.transform.rotation;
        Outlines.SetFloat("_OFFSETX", (transform.position.x * -maskScale) + .5f);
        Outlines.SetFloat("_OFFSETY", (transform.position.y * -maskScale * 1.75f) + .53f);
        //scale is .00035
        yMin = transform.position.y + 339f;
        xMin = transform.position.x - 800f;
        
        
        yMax = transform.position.y + 1000f;
        xMax = transform.position.x + 500f;



        if (Input.GetMouseButtonDown(0))
        {
           
        }
        
        if (Input.GetMouseButtonDown(0) && 
            IsRaycastHitUI(RayCastTarget) && !IsRaycastHitUI(SoundRayCastTarget))
        {
            
            isDragging = true;
           
        }

        // Stop dragging when mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
       
        // Update positions while dragging
        if (isDragging)
        {
            
           
            // TargetPos = Initial ? Vector3.zero : Input.mousePosition;
            //Right Idea but Vector3.zero is wrong value
            TargetPos = new Vector3(Mathf.Clamp(Input.mousePosition.x, 420, ScreenWidthBounds), Mathf.Clamp(Input.mousePosition.y, 500, ScreenHieghtBounds), Input.mousePosition.z);
            
            adjustedY = TargetPos.y - adjustedYOffset;
            Vector3 worldPos = cam2.ScreenToWorldPoint(new Vector3(TargetPos.x, TargetPos.y, distance)); // Use meaningful depth
            gridHolder.transform.position = new Vector3(worldPos.x + XGridOff, worldPos.y + YGridOff, gridHolder.transform.position.z);
            worldPos.z = 0f; // Ensure the object stays in 2D plane or desired depth
            TargetPosPaper = new Vector3(-TargetPos.x + 850, adjustedY - 840, TargetPos.z) * 0.2f + InitialPosPaper;
           // Convert screen space to world space by using Camera.ScreenToWorldPoint
           target.transform.position = new Vector3(TargetPos.x, adjustedY, TargetPos.z);
           RayCastTarget.transform.position = new Vector3(transform.position.x + raycastOffset.x, transform.position.y + raycastOffset.y, transform.position.z);
            
          
            
            
            
        }
        
        //Old Dumb Ass Doinky Transform Method
        
        // Vector3 referenceDirection = mainCamera.transform.forward; // Reference direction (e.g., camera forward)
        // Vector3 perpendicularDir = Vector3.Cross(referenceDirection, Vector3.up).normalized; // Perpendicular to camera forward and world up
        // Vector3 perpendicularDir2 = Vector3.Cross(referenceDirection, Vector3.right).normalized;
        // // Scale the perpendicular direction to determine an offset
        // //this is super dumb and lazy but I can't anymore
        //
        // float offsetMagnitude2 = perpendicularDir2.y < 0 ? ((-TargetPos.y + YGridOff) * Multiplier) / 1000f : ((-TargetPos.y + YGridOff) *-Multiplier) / 1000f;
        //
        // float offsetMagnitude = ((-TargetPos.x + XGridOff) *Multiplier) / 1000; // Adjust as needed for the perpendicular offset
        // Vector3 perpendicularOffset = perpendicularDir * offsetMagnitude;
        // Vector3 perpendicularOffset2 = perpendicularDir2 * offsetMagnitude2;
        //
        // // Apply the offset to world position
        //
        // Vector3 forwardPosition = (mainCamera.transform.position + mainCamera.transform.forward * distance);
        //
        //
        //     gridHolder.transform.position =
        //         forwardPosition + perpendicularOffset + perpendicularOffset2;
        

    }


    public void Minimize()
    {
        
        Outlines.SetFloat("MaskKill", 0f);
        _minimize = true;
        _maximize = false;
        target.SetActive(false);
        RayCastTarget.SetActive(false);
        gridHolder.SetActive(false);
        Tab.SetActive(true);
        
        
        
    }

    public void Maximize()
    {
       
        Outlines.SetFloat("MaskKill", 1f);
        _minimize = false;
        
            target.SetActive(true);
          RayCastTarget.SetActive(true);
            gridHolder.SetActive(true);
            
            Tab.SetActive(false);
        _minimize = false;
        _maximize = true;
        
        
     
    }

    
    private bool IsRaycastHitUI(GameObject target)
    {
        // Create a new PointerEventData and set the mouse position
        pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        // Raycast and store the results
        var results = new System.Collections.Generic.List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        // Check if the target is in the raycast results
        foreach (var result in results)
        {
            if (result.gameObject == target)
            {
                
                
                return true;
            }
        }

        return false;
    }

    private IEnumerator Time()
    {
        yield return null;
        isDragging = false;
    }

    private IEnumerator StartJult()
    {
        isDragging = true;
        yield return null;
        isDragging = false;
        Initial = false;
    }
}