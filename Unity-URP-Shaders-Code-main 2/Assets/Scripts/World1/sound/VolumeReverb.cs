using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeReverb : MonoBehaviour
{
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    private GameObject SoundKnob;
    private bool IsDragging;
    private bool CoroutineIsRunning = false;
    [SerializeField] private float Xmax, Xmin, Ymax, Ymin;
    private GameObject SoundHud2;
    [SerializeField] private AudioMixer mixer;

    void Start()
    {
        
        if (mixer == null)
        {
            Debug.LogWarning("SoundHud2 AudioMixer not found");
        }
        // Initialize GraphicRaycaster and EventSystem
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        if (graphicRaycaster == null)
        {
            Debug.LogError("GraphicRaycaster not found. Ensure there is a Canvas with a GraphicRaycaster in the scene.");
        }

        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            Debug.LogError("EventSystem not found. Ensure there is an EventSystem in the scene.");
        }

        // Initialize SoundKnob
        SoundKnob = GameObject.Find("SoundButton");
        if (SoundKnob == null)
        {
            Debug.LogError("SoundKnob is null. Ensure there is a GameObject named 'SoundButton' in the scene.");
        }
        SoundHud2 = GameObject.Find("SoundHUDPt2");
        if (SoundHud2 == null)
        {
            Debug.LogError("No SoundHUD2");
        }
    }

    void Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            if (SoundKnob == null)
            {
                SoundKnob = GameObject.Find("SoundButton");
            }

            if (SoundHud2 == null)
            {
                SoundHud2 = GameObject.Find("SoundHUDPt2");
            }

            if (graphicRaycaster != null && eventSystem != null && IsRaycastHitUI(SoundKnob))
            {
               IsDragging = true;
               
            }

            

            if (IsDragging)
            {
                float xPos;
                float yPos;
                
                float XHud = SoundHud2.transform.position.x;
                float YHud = SoundHud2.transform.position.y;
                xPos = Mathf.Clamp(Input.mousePosition.x, Xmin + XHud, Xmax + XHud);
                yPos = Mathf.Clamp(Input.mousePosition.y, Ymin + YHud, Ymax + YHud);
                
                Vector3 pos = new Vector3(xPos, yPos, SoundKnob.transform.position.z);
                float normalizedY = Mathf.InverseLerp(Ymin + YHud, Ymax + YHud, yPos); 
                float VolumeAmount = Mathf.Lerp(-80f, 0f, Mathf.Log10(1 + (9 * normalizedY)));
                float normalizedX = Mathf.InverseLerp(Xmin + XHud, Xmax + XHud, xPos);
                float ReverbAmount = Mathf.Lerp(-80, 0f, Mathf.Log10(1 + (9 * normalizedX)));
               
               

                SoundKnob.transform.position = pos;
                mixer.SetFloat("Volume", VolumeAmount);
                mixer.SetFloat("WetAmount", ReverbAmount);
             
            }
        }
        if (IsDragging && !Input.GetMouseButton(0))
        {
            IsDragging = false;
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
                return true;
            }
        }

        return false;
    }

    private IEnumerator DragWait()
    {
        CoroutineIsRunning = true;
        yield return new WaitForSeconds(.3f);
        IsDragging = false;
        CoroutineIsRunning = false;
    }
}
