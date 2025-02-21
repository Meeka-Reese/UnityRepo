using System.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteDrag : MonoBehaviour
{
    [SerializeField] private GameObject RayCastTarget;
    [SerializeField] private float AdjustedY = 0;
    public int Index;
    private GameObject Layer2;
    public bool IndivDragging = false;
    private NoteManager noteManager;
   
   
    public bool isDragging = false;
    
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
        

    private void Start()
    {
        noteManager = FindObjectOfType<NoteManager>();
        Layer2 = GameObject.Find("Layer2");
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
        
    }
    private void Update()
    {
        Debug.Log(noteManager.MultDragging);
        if (Input.GetMouseButtonDown(0) && IsRaycastHitUI(RayCastTarget))
        {
            StartCoroutine(LittleDelay());

        }
        if (Input.GetMouseButtonUp(0))
        {
            IndivDragging = false;
            isDragging = false;
        }

        if (isDragging)
        {
            transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + AdjustedY, transform.position.z);
        }
    }
    private bool IsRaycastHitUI(GameObject target)
    {
        IndivDragging = false;
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
                
                IndivDragging = true;
                return true;
                
                    
                
            }
            
        }

        return false;
    }

    private IEnumerator LittleDelay()
    {
        
        yield return new WaitForSeconds(0.01f);
        
        
            if ((noteManager.MultDragging))
            {
                if (transform.parent == Layer2.transform)
                {
                    isDragging = true;
                }
                else
                {
                    isDragging = false;
                }
            }
            else if (!noteManager.MultDragging)
            {
                isDragging = true;
            }
        
    }
}
    
