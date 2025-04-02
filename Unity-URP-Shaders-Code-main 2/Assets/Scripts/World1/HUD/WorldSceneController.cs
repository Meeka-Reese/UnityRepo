using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSceneController : MonoBehaviour
{

    public void Bootup()
    {
        SceneManager.LoadScene(0);
    }
}
