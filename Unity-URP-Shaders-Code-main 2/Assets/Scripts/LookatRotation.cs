using UnityEngine;

public class LookatRotation : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
      transform.LookAt(Camera.main.transform);
      transform.rotation = Quaternion.Euler(90f, transform.eulerAngles.y + 180, 180f);
    }
}
