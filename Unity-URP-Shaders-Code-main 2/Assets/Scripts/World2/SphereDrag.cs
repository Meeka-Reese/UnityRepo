using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SphereDrag : MonoBehaviour
{
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    [SerializeField] private Camera MainCamera;
    [SerializeField] private GameObject Target;
    private bool Holding = false;
    void Start()
    {
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRaycastHit(Target) || Holding)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 30;
            Target.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Holding = false;
        }
        
    }
    private bool IsRaycastHit(GameObject target)
    {
        if (Input.GetMouseButton(0)) // Check if left mouse button is clicked
        {
            
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == target)
                {
                    Holding = true;
                    return true;
                }
            }
        }
        return false;
    }
}
