using System;
using System.Collections.Generic;
using System.Linq;
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
   public Dictionary<string, int> ColorAmount;
    public string CurrentColorName = "Black";
    public bool PurpleFlower = false;
    
    void Awake()
    {
        _shaderChangeHandler = FindObjectOfType<ShaderChangeHandler>();
        GridHolder = GameObject.Find("Grid Holder");
        ColorAmount = new Dictionary<string, int>
        {
            {"Purple", 0},
            {"Yellow",0},
            {"Green",0},
            {"Brown",0},
            {"Black",0},
            {"Grey",0},
            {"Blue",0},
            {"Red",0},
            {"Orange",0},
            {"White",0},
            {"Pink",0},
            {"",0}
        };
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
       // Debug.Log(ColorAmount["Purple"] + ":Purple" + ColorAmount["Yellow"] + ":Yellow" + ColorAmount["Green"] + ": Green");

        if (ColorAmount["Purple"] > 150 && ColorAmount["Yellow"] > 10 && ColorAmount["Green"] > 150)
        {
            PurpleFlower = true;
        }
        else
        {
            PurpleFlower = false;
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
            EraseColorSwap = false;
            _shaderChangeHandler.Erase = false;
            CurrentColor = color.GetComponent<Image>().color;
            CurrentColorName = color.gameObject.name;
            
            
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

    public void Bomb()
    {
        foreach (var key in ColorAmount.Keys.ToList())
        {
            ColorAmount[key] = 0;
        }
    }

    public void Bucket()
    {
        // Reset all existing colors
        foreach (var key in ColorAmount.Keys.ToList())
        {
            ColorAmount[key] = 0;
        }

        // Safely set the current color
        if (ColorAmount.ContainsKey(CurrentColorName))
        {
            ColorAmount[CurrentColorName] = 4800;
        }
        else
        {
            Debug.LogError($"Color '{CurrentColorName}' not found in dictionary!");
            
            
        }
    }
}
