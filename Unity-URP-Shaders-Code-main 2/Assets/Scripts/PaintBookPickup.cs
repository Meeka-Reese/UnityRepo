using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PaintBookPickup : MonoBehaviour
{
    [Header("Paint Objects and Shit")] 
    [SerializeField] private GameObject PaintTab;
    [SerializeField] private GameObject PaintBook;
    [SerializeField] private GameObject RotatingPaintBook;

    [SerializeField] private GameObject RaycastTarget;
    private GameObject Player;
    private DialogueTrigger dialogueTrigger;
    private Ray ray;
    private RaycastHit hit;
    private Camera MainCamera;
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    private GameObject GridHolder;
    private GameObject PaintAnimation;
    private bool CoroutineRunning = false;
    private bool BookPickedUp = false;
    private GameObject PaintTabMinimized;
    private GameObject ItemCollect;
    private GameObject ScreenshotHolder;
    private SoundHandler soundHandler;

    
  
    void Start()
    {
        RaycastTarget.SetActive(false);
        PaintTab.SetActive(false);
        RotatingPaintBook.SetActive(false);
        Player = GameObject.FindGameObjectWithTag("Player");
        dialogueTrigger = GameObject.Find("DialogueTrigger").GetComponent<DialogueTrigger>();
        MainCamera = Camera.main;
        GridHolder = GameObject.Find("Grid Holder");
        PaintAnimation = GameObject.Find("PaintAnimation");
        GridHolder.SetActive(false);
        PaintAnimation.SetActive(false);
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
        PaintTabMinimized = GameObject.Find("Tab");
        PaintTabMinimized.SetActive(false);
        ItemCollect = GameObject.Find("ItemCollect");
        ItemCollect.SetActive(false);
        ScreenshotHolder = GameObject.Find("ScreenshotHolder");
        ScreenshotHolder.SetActive(false);
        soundHandler = GameObject.Find("SoundHandler").GetComponent<SoundHandler>();
    }

    
    void Update()
    {
        
        float distance = Vector3.Distance(Player.transform.position, RaycastTarget.transform.position);
        
        if (distance < 3000 && !CoroutineRunning && !BookPickedUp)
        {
            RaycastTarget.SetActive(true);
            if (IsRaycastHit(RaycastTarget))
            {
                StartCoroutine(PaintGrabbed());
            }  
        }
        else
        {
            RaycastTarget.SetActive(false);
        }
        
         
        
    }

    private IEnumerator PaintGrabbed()
    {
        ItemCollect.SetActive(true);
        ItemCollect.GetComponent<TextMeshProUGUI>().text = "- Paint Book Collected -";
        CoroutineRunning = true;
        RaycastTarget.SetActive(false);
        RotatingPaintBook.SetActive(true);
        soundHandler.ChangeTrigger("BgVolume", "BgAltVolume");
        while (!Input.GetKeyDown(KeyCode.I))
        {
            RotatingPaintBook.transform.rotation =
                Quaternion.Euler(RotatingPaintBook.transform.eulerAngles.x, 
                    RotatingPaintBook.transform.eulerAngles.y + 1,
                    RotatingPaintBook.transform.eulerAngles.z);
            yield return null;
        }
        ItemCollect.GetComponent<TextMeshProUGUI>().text = "";
        ItemCollect.SetActive(false);
        ScreenshotHolder.SetActive(true);
        PaintTab.SetActive(true);
        GridHolder.SetActive(true);
        PaintAnimation.SetActive(true);
        RotatingPaintBook.SetActive(false);
        CoroutineRunning = false;
        BookPickedUp = true;
        PaintTabMinimized.SetActive(true);
        PaintBook.SetActive(false);
        soundHandler.ChangeTrigger("BgAltVolume", "BgVolume");
    }
    private bool IsRaycastHit(GameObject target)
    {
        if (Input.GetMouseButtonDown(0)) // Check if left mouse button is clicked
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == target)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
}
