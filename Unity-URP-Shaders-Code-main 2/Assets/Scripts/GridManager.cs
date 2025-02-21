using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameObject pixelObject;
    public int GridHeight = 80;
    public Color CurrentColor = Color.black;
    public int GridWidth = 60;
    public float GridMult = 2f;
    [SerializeField] private GameObject GridHolder;
    private ShaderChangeHandler _shaderChangeHandler;
    private bool erase = false;
    [SerializeField] private Quaternion AngleOffset;
    private Vector3 GridPos;
    public List<GameObject> Pixels;
    private Color LastColor;
    private bool EraseColorSwap = false;
    void Awake()
    {
        _shaderChangeHandler = FindObjectOfType<ShaderChangeHandler>();
        GridHolder = GameObject.Find("Grid Holder");
        
        GridPos = new Vector3(GridHolder.transform.position.x, GridHolder.transform.position.y, GridHolder.transform.position.z);
        SpawnGrid();
    }

    private void Update()
    {
        erase = _shaderChangeHandler.Erase;
        if (erase)
        {
            SetEraseColor();
        }

        if (erase && CurrentColor != Color.white)
        {
            _shaderChangeHandler.Sphere();
        }

        if (!erase && EraseColorSwap)
        {
            SetPreviousColor();
        }
        
        
    }

    void SpawnGrid()
    {
        Pixels = new List<GameObject>();
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                GameObject obj = Instantiate(pixelObject, (new Vector3((GridMult * x), (GridMult * y), 0) + GridPos), AngleOffset, transform);
                obj.transform.parent = GridHolder.transform;
                Pixels.Add(obj);
            }
        }
    }

    public void SetPencilColor(GameObject color)
    {
        
            CurrentColor = color.GetComponent<Image>().color;
            
            LastColor = CurrentColor;
    }

    private void SetEraseColor()
    {
        
        CurrentColor = Color.white;
        //current bug where erasor breaks and turns into checker
        EraseColorSwap = true;
    }

    private void SetPreviousColor()
    {
        CurrentColor = LastColor;
        EraseColorSwap = false;
    }

    public Color GetCurrentColor()
    {
        return CurrentColor;
    }
}
