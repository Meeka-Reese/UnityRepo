using UnityEngine;

public class InheritPos : MonoBehaviour
{
    [SerializeField] GameObject Parent;
    void Start()
    {
        
        
    }

    
    void Update()
    {
        transform.position = new Vector3(Parent.transform.position.x, Parent.transform.position.y, Parent.transform.position.z - 300f);
    }
}
