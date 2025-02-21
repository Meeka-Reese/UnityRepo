using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StickerInstantiate : MonoBehaviour
{
    [SerializeField] private GameObject StickerParent;
    private GameObject currentSticker;
    private bool stickerCreation = false;
    public bool StickerCreateDelay = false;
    [SerializeField] private GameObject RayCastTarget;
    public float StickerSize;

    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    private void Start()
    {
        
        if (RayCastTarget == null)
        {
            Debug.LogError("RayCastTarget is not assigned in the Inspector.");
            return;
        }
        RayCastTarget.SetActive(false);

        // Find the GameObject named "Stickers" to act as the parent
        if (StickerParent == null)
        {
            Debug.LogError("StickerParent is not assigned in the Inspector.");
        }

        // Get the GraphicRaycaster from the Canvas
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
    }

    public void InstantiateSticker(GameObject stickerPrefab)
    {
        Debug.Log(stickerPrefab);
        if (!stickerCreation)
        {
            RayCastTarget.SetActive(true);
            // Instantiate the sticker and set its parent to StickerParent
            currentSticker = Instantiate(stickerPrefab, Vector3.zero, Quaternion.identity, StickerParent.transform);
            currentSticker.tag = "Sticker";
            stickerCreation = true;
            StickerCreateDelay = true;
        }
    }

    private void Update()
    {
        if (stickerCreation && currentSticker != null)
        {
            currentSticker.SetActive(true);
            // Make the sticker follow the mouse position
            currentSticker.transform.position = Input.mousePosition;
            currentSticker.transform.localScale = StickerSize * Vector3.one;

            // Place the sticker when the mouse is clicked
            if (Input.GetMouseButtonDown(0) && IsRaycastHitUI(RayCastTarget))
            {
                stickerCreation = false;
                currentSticker = null;
            }
        }
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
                RayCastTarget.SetActive(false);
                StartCoroutine(DelaySticker());
                return true;
            }
        }

        return false;
    }

    private IEnumerator DelaySticker()
    {
        yield return new WaitForSeconds(0.5f);
        
        StickerCreateDelay = false;
    }
}
