using UnityEngine;

public class SkyboxMove : MonoBehaviour
{
    [SerializeField] private Material SkyboxMaterial;
    float offset;
    

  
    void Update()
    {
        offset += Time.deltaTime * .01f;
        SkyboxMaterial.SetFloat("_offset", offset);
    }
}
