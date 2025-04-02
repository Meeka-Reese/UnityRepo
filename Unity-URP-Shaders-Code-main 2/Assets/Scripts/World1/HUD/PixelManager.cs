 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelManager : MonoBehaviour
{
    private Renderer pren;
   private MouseDragScript mouseDragScript;
   private SoundDrag soundDrag;
    private ShaderChangeHandler shaderChangeHandler;
    private StickerInstantiate stickerInstantiate;
    private bool mouseDragging;
    private bool BrushSizeChanging = false;
    private MaterialPropertyBlock propertyBlock;
    private bool actionPending = false;
    private bool CoRoutineIsRunning = false;
    private bool CoRoutineIsRunningColor = false;
    private bool StickerActive = false;
    private bool Checker = false;
    private bool Bombed = false;
    private bool soundDragging = false;
    private bool minimizing = false;
    private GameObject GridHolderParent;
    private float brushRadius = 0.5f;
    private bool Bucket = false;
    private string PixelColor = "Black";
    private GridManager gridManager;
    private Dictionary<int, Color> UndoCue;
    private int CueIndex = 1;
    

    private void Awake()
    {
        
        mouseDragScript = FindObjectOfType<MouseDragScript>();
        soundDrag = FindObjectOfType<SoundDrag>();
        stickerInstantiate = FindObjectOfType<StickerInstantiate>();
        shaderChangeHandler = FindObjectOfType<ShaderChangeHandler>();
        pren = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
        GridHolderParent = GameObject.Find("GridHolderParent");
        gridManager = GridHolderParent.GetComponent<GridManager>();
        UndoCue = new Dictionary<int, Color>()
        {
            {0, Color.white}
        };

    }

    private void Update()
    {
        
        minimizing = soundDrag.Minimizing;
        soundDragging = soundDrag.isDragging;
        Bombed = shaderChangeHandler.Bombed;
        Bucket = shaderChangeHandler.BucketFills;
        Checker = shaderChangeHandler.IsChecker;
        StickerActive = stickerInstantiate.StickerCreateDelay;
        BrushSizeChanging = shaderChangeHandler.isSliderChanging;
        mouseDragging = mouseDragScript.isDragging;
        if (Bombed)
        {
            
            propertyBlock.SetColor("_Color", Color.white);
            pren.SetPropertyBlock(propertyBlock);
            if (!CoRoutineIsRunning)
            {
                StartCoroutine(Wait(Bombed));
            }

        }

        if (Bucket)
        {
            propertyBlock.SetColor("_Color", GridHolderParent.GetComponent<GridManager>().CurrentColor);
            pren.SetPropertyBlock(propertyBlock);
            if (!CoRoutineIsRunning)
            {
                StartCoroutine(Wait(Bucket));
                
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            UndoCue[CueIndex] = GridHolderParent.GetComponent<GridManager>().CurrentColor;
            CueIndex++;
        }

        
        if ((Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand)) && Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Command Z Pressed");
            
            if (CueIndex > 0)
            {
                CueIndex--;
                if (UndoCue.ContainsKey(CueIndex))
                {
                    propertyBlock.SetColor("_Color", UndoCue[CueIndex]);
                    pren.SetPropertyBlock(propertyBlock);
                }
            }
            
            //Command z function
        }

        
    }

    private void OnTriggerStay(Collider other)
    {
       
        if (!actionPending && other.CompareTag("Brush") && !StickerActive && !CoRoutineIsRunningColor)
        {
            StartCoroutine(HandleBrushInteraction(other));
        }
    }

    private IEnumerator HandleBrushInteraction(Collider other)
    {
        CoRoutineIsRunningColor = true;
        actionPending = true;
        
        String PreviousName = PixelColor; 
        PixelColor = gridManager.CurrentColorName;
    
        
        
        yield return null;
        
        if (!mouseDragging && !soundDragging && !minimizing && !BrushSizeChanging && Input.GetMouseButton(0))
        {
            if (gridManager.ColorAmount.ContainsKey(PixelColor) && PreviousName != PixelColor && gridManager.ColorAmount[PixelColor] < 4800)
            {
                    gridManager.ColorAmount[PixelColor]++;
                    
                if (gridManager.ColorAmount.ContainsKey(PreviousName) && gridManager.ColorAmount[PreviousName] > 0)
                {
                        gridManager.ColorAmount[PreviousName]--;
                }
                
            }
            
            
            // Get the pixel's position in the grid
            Vector3 position = transform.localPosition; // Local to the grid's parent
            int x = Mathf.FloorToInt(position.x / GridHolderParent.GetComponent<GridManager>().GridMult);
            int y = Mathf.FloorToInt(position.y / GridHolderParent.GetComponent<GridManager>().GridMult);

            // If Checker mode is on, check the checkerboard condition
            if (!Checker || (x + y) % 2 == 0)
            {
                // Debug.Log(transform.parent.GetComponent<GridManager>().CurrentColor);
                propertyBlock.SetColor("_Color", GridHolderParent.GetComponent<GridManager>().CurrentColor);
                pren.SetPropertyBlock(propertyBlock);
            }
        }

        actionPending = false;
        CoRoutineIsRunningColor = false;
    }

    private IEnumerator Wait(bool message)
    {
        CoRoutineIsRunning = true;
        yield return new WaitForSeconds(0.5f);
        message = false;
        shaderChangeHandler.Bombed = Bombed;
        shaderChangeHandler.BucketFills = Bucket;
        CoRoutineIsRunning = false;
        yield return null;
    }
    

}