using UnityEngine;
using UnityEngine.UI;

public class ShaderChangeHandler : MonoBehaviour
{
    
   [SerializeField] private Material Cursormaterial;
   [SerializeField] private Slider Slider;
   [SerializeField] private Mesh SphereMesh;
   [SerializeField] private Mesh CheckerMesh;
   private MeshCollider _meshCollider;
   private MeshFilter _meshFilter;
   private StickerInstantiate _stickerInstantiate;
   private float BrushSize = 1f;
   public bool isSliderChanging = false;
   public bool IsChecker = false;
   public bool Erase = false;
   public bool Bombed = false;
   
    

    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        _stickerInstantiate = FindObjectOfType<StickerInstantiate>();
        Slider.onValueChanged.AddListener(ChangeBrushSize);
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
    }
    public void ChangeBrushSize(float value)
    {
         BrushSize = value * .4f;
         transform.localScale = BrushSize * Vector3.one;
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
        Erase = false;
        IsChecker = false;
        BrushChange(SphereMesh);
    }

    public void Checker()
    {
        Erase = false;
        IsChecker = true;
        BrushChange(CheckerMesh);
    }

    public void Eraser()
    {
        Debug.Log("EraserFunctions");
        IsChecker = false;
        Erase = true;
        Debug.Log(Erase);
        BrushChange(SphereMesh);
        
    }

    public void Bomb()
    {
        Bombed = true;
        GameObject[] ObjectsToDelete = GameObject.FindGameObjectsWithTag("Sticker");
        foreach (GameObject obj in ObjectsToDelete)
        {
            Destroy(obj);
        }
    }
    private void BrushChange(Mesh mesh)
    {
        _meshCollider.sharedMesh = mesh;
        _meshFilter.mesh = mesh;
    }

    
        
    
    
}
