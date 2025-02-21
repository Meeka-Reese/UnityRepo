using System.Collections;
using UnityEngine;

public class WaterFallMove : MonoBehaviour
{
    private Material WaterFallMaterial;
    [SerializeField] private float TimeScale = 1f;

    private float DumbTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WaterFallMaterial = GetComponent<MeshRenderer>().material;
        StartCoroutine(DownFrame());
    }

    
    void Update()
    {
        
    }

    private IEnumerator DownFrame()
    {
        while (true)
        {
            WaterFallMaterial.SetVector("_Movement", new Vector2(0, DumbTime));
            yield return new WaitForSeconds(.0833f);
            DumbTime += TimeScale;

        }
    }
}
