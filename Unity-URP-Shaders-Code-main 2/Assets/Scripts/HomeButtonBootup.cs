using UnityEngine;

public class HomeButtonBootup : MonoBehaviour
{
    private GameObject Message;

    void Awake()
    {
        Message = GameObject.Find("Message");
        Message.SetActive(false);
    }

    public void HomeClick()
    {
        Message.SetActive(true);
    }

    public void XClick()
    {
        Message.SetActive(false);
    }
}
