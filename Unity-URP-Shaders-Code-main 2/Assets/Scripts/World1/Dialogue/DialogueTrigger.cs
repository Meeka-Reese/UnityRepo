using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueTrigger : MonoBehaviour
{
    [Header("VisualCue")]
    [SerializeField] private GameObject visualCue;
    [Header("Ignore Layers")] 
    [SerializeField] private LayerMask ignoreLayer1;
    [SerializeField] private LayerMask ignoreLayer2;
    private LayerMask combinedIgnoreLayers;
    [Header("RayHitStuff")] 
    public float MaxTalkDistance = 3000;
    [SerializeField] private string TagName;
    private Ray ray;
    private RaycastHit hit;
    private Camera MainCamera;
    [Header("Ink JSON")] 
    [SerializeField] private TextAsset InkJson;

    [SerializeField] private GameObject[] Choices;

    private GameObject Player;
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    private Vector3 StartScale;
    private float ScaleSin;
    private GameObject Layer1;
    private GameObject Layer2;
    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip ButtonHoverSound;
    [SerializeField] private AudioClip ButtonClickSound;
    private GameObject currentGame;
   

    private void Start()
    {
        currentGame = GameObject.Find("Canvas");
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
        visualCue.SetActive(false);
        Player = GameObject.FindGameObjectWithTag("Player");
        MainCamera = Camera.main;
        combinedIgnoreLayers = ignoreLayer1 | ignoreLayer2;
        StartScale = Choices[0].transform.localScale;
        Layer1 = GameObject.Find("Layer1");
        Layer2 = GameObject.Find("Layer2");
        audioSource = GetComponent<AudioSource>();
        
    }

    private void Update()
    {
        //kill if too far away
        float distance = Vector3.Distance(Player.transform.position, transform.position);
        
        if (DialogueManager.GetInstance().isPlaying)
        {
            visualCue.SetActive(false);
        }
        else if (!DialogueManager.GetInstance().isPlaying && (distance < MaxTalkDistance))
        {
           
            visualCue.SetActive(true);
        }

        int index = 0;
        bool scaling = false;
            
        //Scale change on mouse over to indicate selection
        foreach (GameObject choice in Choices)
        {
            if (IsRaycastHitUI(Choices[index]) && !scaling)
            {
                if (currentGame != choice)
                {
                    audioSource.PlayOneShot(ButtonHoverSound);
                }
                scaling = true;
                Choices[index].transform.localScale = (StartScale * 11) / 10;
                Choices[index].transform.parent = Layer2.transform;
                currentGame = choice;
                if (Input.GetMouseButtonDown(0))
                {
                    audioSource.PlayOneShot(ButtonClickSound);
                }
                
               
            }
            else
            {
                Choices[index].transform.localScale = StartScale;
                Choices[index].transform.parent = Layer1.transform;
            }
            index++;
            
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            ray = MainCamera.ScreenPointToRay(Input.mousePosition);

            // Perform raycast and ignore objects on the "Notification" layer
            
            if (Physics.Raycast(ray, out hit, MaxTalkDistance, ~combinedIgnoreLayers))
            {
                if (hit.collider != null && hit.collider.gameObject.CompareTag(TagName) && !DialogueManager.GetInstance().isPlaying)
                {
                    DialogueManager.GetInstance().StartDialogue(InkJson);

                }
                
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
                
                
                return true;
            }
        }

        return false;
    }
    
}
