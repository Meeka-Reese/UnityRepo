using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    [SerializeField] private Material LineMat;

    
    void Color1 ()
    {
        LineMat.SetFloat("_Red", .878f);
        LineMat.SetFloat("_Green", 187);
        LineMat.SetFloat("_Blue", 228);
    }
}
    