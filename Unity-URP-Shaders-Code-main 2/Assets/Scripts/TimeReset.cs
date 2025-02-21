using Unity.VisualScripting;
using UnityEngine;

public class TimeReset : MonoBehaviour
{
    private Material PaintDot;
    private float Times;
    private float Intervals;
    void Start()
    {
        PaintDot = GetComponent<MeshRenderer>().material;
    }

    
    void Update()
    {
        Times = Time.deltaTime;
    }
}
