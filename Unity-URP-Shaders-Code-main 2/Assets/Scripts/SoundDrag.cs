using System.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundDrag : MonoBehaviour
{
    [SerializeField] private GameObject target;
    
    
    private GameObject Tab;
    
    private float journeyLength;
    private float startTime;
    private Vector3 MinimizePos;
    private Vector3 lastVector3;
    private Vector3 lastScaleTarget;
    private Vector3 lastScalePaper;
    
    private GameObject SoundHUDPt2;
    private bool Pt2Open = false;
    
    private Vector3 Dragpos;
    private Vector3 screenPos;

    [Header("Values For Tweaking Drag")] 
    
    
    [SerializeField] private float adjustedYOffset;
    [SerializeField] private float adjustedXOffset;

    [Header("Bounds Of Dragging")] 
    [SerializeField] private float xDragMax;
    [SerializeField] private float xDragMin;
    [SerializeField] private float yDragMax;
    [SerializeField] private float yDragMin;

    [Header("Everything Else")] 
    
    

    private float adjustedY;
    private float xBoundMin;
    private float xBoundMax;
    private float yBoundMin;
    private float yBoundMax;
    private bool _maximize;
    private Camera mainCamera;
    public bool isDragging;
    public bool Minimizing = false;
    
    
    
    private Vector3 TargetPos;
    
    
    
   
    [SerializeField] private float TransitionSpeed = .55f;
    public float maskScale;
    public bool _minimize;
    private Vector3 minimizeStartPos;
    private Vector3 minimizeTargetPos;
    private Vector3 targetScaleTarget = Vector3.zero;
  
    
 
    private float transitionProgress = 0f;
    private Vector3 initialScaleTarget;
   
    private GameObject RayCastTarget;
    private Camera cam;
    
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    private void Start()
    {
        
        SoundHUDPt2 = GameObject.Find("SoundHUDPt2");
        SoundHUDPt2.SetActive(false);
        RayCastTarget = GameObject.Find("RaycastSound");
        RayCastTarget.SetActive(true);
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
        cam = Camera.main;
       _maximize = false;
        _minimize = false;
        mainCamera = Camera.main;
       
       
        target = GameObject.Find("SoundTab");
        Tab = GameObject.Find("Tab2");
        Tab.SetActive(false);
       
       
        minimizeStartPos = target.transform.position;

        
        initialScaleTarget = target.transform.localScale;
       
        
        lastScaleTarget = target.transform.localScale;

       

        // Initialize TargetPos to the target's starting position
        TargetPos = target.transform.position;
       

        
        
    }

    private void Update()
    {
     
        if (Input.GetMouseButtonDown(0))
        {
            
        }
        
        if (Input.GetMouseButtonDown(0) && 
            IsRaycastHitUI(RayCastTarget))
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
            float xPos = Mathf.Clamp(Input.mousePosition.x, xDragMin, xDragMax);
            float yPos = Mathf.Clamp(Input.mousePosition.y, yDragMin, yDragMax);
            float zPos = Input.mousePosition.z;
            Vector3 PosBounds = new Vector3(xPos, yPos, zPos);
            TargetPos = PosBounds;
            adjustedY = TargetPos.y - adjustedYOffset;
            float adjustedX = TargetPos.x - adjustedXOffset;
           
            
           // Convert screen space to world space by using Camera.ScreenToWorldPoint
           target.transform.position = new Vector3(adjustedX, adjustedY, TargetPos.z);
           
            
            
            
            
        }
        Vector3 referenceDirection = mainCamera.transform.forward; // Reference direction (e.g., camera forward)
        Vector3 perpendicularDir = Vector3.Cross(referenceDirection, Vector3.up).normalized; // Perpendicular to camera forward and world up
        Vector3 perpendicularDir2 = Vector3.Cross(referenceDirection, Vector3.right).normalized;
        // Scale the perpendicular direction to determine an offset
        //this is super dumb and lazy but I can't anymore
        
       
        
       
        

       
       
    }


    public void Minimize()
    {
        
        _minimize = true;
        target.SetActive(false);
        RayCastTarget.SetActive(false);
        Tab.SetActive(true);
        //Glitch to look into, when minimizing over drawing pad, drawing pad draws dot
    
    }

    public void Maximize()
    {
        
        _minimize = false;
       
            target.SetActive(true);
          RayCastTarget.SetActive(true);
           
            
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

    public void HUDPt2()
    {
        Pt2Open = !Pt2Open;
        if (Pt2Open)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    private void Open()
    {
        SoundHUDPt2.SetActive(true);
    }

    private void Close()
    {
        SoundHUDPt2.SetActive(false);
    }
}