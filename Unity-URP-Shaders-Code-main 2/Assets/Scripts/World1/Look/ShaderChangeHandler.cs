using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShaderChangeHandler : MonoBehaviour
{
    
   [SerializeField] private Material Cursormaterial;
   [SerializeField] private Slider Slider;
   [SerializeField] private Mesh SphereMesh;
   [SerializeField] private Mesh CheckerMesh;
   [SerializeField] private GameObject RayCastTarget;
   private GameObject MouseColliderSlow;
   private GameObject MouseColliderSlower;
   private MeshCollider _meshCollider;
   private MeshFilter _meshFilter;
   private StickerInstantiate _stickerInstantiate;
   private float BrushSize = 1f;
   public bool isSliderChanging = false;
   public bool IsChecker = false;
   public bool Erase = false;
   public bool Bombed = false;
   public bool BucketFills = false;
   [SerializeField] private bool BucketSelect  = false;
   private GridManager _gridManager;
   private GraphicRaycaster graphicRaycaster;
   private PointerEventData pointerEventData;
   private EventSystem eventSystem;
   [Header("Mouse Icons")]
   [SerializeField] private Texture2D MouseIcon;

   [SerializeField] private Texture2D BucketIcon;
   [SerializeField] private GameObject MouseCursor;
   
    

    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        _stickerInstantiate = FindObjectOfType<StickerInstantiate>();
        Slider.onValueChanged.AddListener(ChangeBrushSize);
        MouseColliderSlow = GameObject.Find("MouseColliderSlow");
        MouseColliderSlower = GameObject.Find("MouseColliderSlower");
        _gridManager = GameObject.Find("GridHolderParent").GetComponent<GridManager>();
        eventSystem = FindObjectOfType<EventSystem>();
        graphicRaycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        MouseCursor.GetComponent<RawImage>().texture = MouseIcon;
        


    }

    private void Update()
    {
        _stickerInstantiate.StickerSize = Slider.value * .05f;
        if (Input.GetMouseButton(0))
        {
            
            Cursormaterial.SetFloat("_Alpha", 0f);
        }

        if (!Input.GetMouseButton(0))
        {
            Cursormaterial.SetFloat("_Alpha", .5f);
        }

        if (BucketSelect && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Bucket");
            _gridManager.Bucket();
            BucketFills = true;
            StartCoroutine(BucketEnd());
        }
    }
    public void ChangeBrushSize(float value)
    {
         BrushSize = value * .4f;
         transform.localScale = BrushSize * Vector3.one;
         MouseColliderSlow.transform.localScale = BrushSize * Vector3.one;
         MouseColliderSlower.transform.localScale = BrushSize * Vector3.one;
         isSliderChanging = true;
         CancelInvoke(nameof(ResetSliderChanging)); // Cancel any previously scheduled reset
         Invoke(nameof(ResetSliderChanging), 0.5f); // Delay to reset the bool
    }
    private void ResetSliderChanging()
    {
        // Reset the bool after the delay
        isSliderChanging = false;
    }

    public void Sphere()
    {
        BucketFills = false;
        Erase = false;
        BucketSelect = false;
        IsChecker = false;
        BrushChange(SphereMesh);
    }

    public void Checker()
    {
        Erase = false;
        BucketFills = false;
        BucketSelect = false;
        IsChecker = true;
        BrushChange(CheckerMesh);
    }

    public void BucketFill()
    {
        StartCoroutine(BucketFillCount());
    }

    private IEnumerator BucketFillCount()
    {
        yield return new WaitForSeconds(0.1f);
        BucketSelect = true;
        Bombed = false;
        Erase = false;
        MouseCursor.GetComponent<RawImage>().texture = BucketIcon;
        
    }

    private IEnumerator BucketEnd()
    {
        yield return new WaitForSeconds(0.1f);
        BucketFills = false;
        BucketSelect = false;
        MouseCursor.GetComponent<RawImage>().texture = MouseIcon;
        
        
    }

    public void Eraser()
    {
        Debug.Log("EraserFunctions");
        IsChecker = false;
        BucketFills = false;
        BucketSelect = false;
        Erase = true;
        Debug.Log(Erase);
        BrushChange(SphereMesh);
        
    }

    public void Bomb()
    {
        Bombed = true;
        BucketFills = false;
        BucketSelect = false;
        _gridManager.Bomb();
        
        GameObject[] ObjectsToDelete = GameObject.FindGameObjectsWithTag("Sticker");
        foreach (GameObject obj in ObjectsToDelete)
        {
            Destroy(obj);
        }

        StartCoroutine(BombWait());

    }
    private void BrushChange(Mesh mesh)
    {
        _meshCollider.sharedMesh = mesh;
        MouseColliderSlow.GetComponent<MeshCollider>().sharedMesh = mesh;
        MouseColliderSlower.GetComponent<MeshCollider>().sharedMesh = mesh;
        _meshFilter.mesh = mesh;
        MouseColliderSlow.GetComponent<MeshFilter>().mesh = mesh;
        MouseColliderSlower.GetComponent<MeshFilter>().mesh = mesh;
    }

    private IEnumerator BombWait()
    {
        yield return new WaitForSeconds(0.4f);
        Bombed = false;
    }


    
        
    
    
}
