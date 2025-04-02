using UnityEngine;

public class NoiseShaderMove : MonoBehaviour
{
    private Material noiseMaterial;

    private void Start()
    {
        noiseMaterial = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        noiseMaterial.SetFloat("_TimeOffset", Time.time / 40);
    }
}
