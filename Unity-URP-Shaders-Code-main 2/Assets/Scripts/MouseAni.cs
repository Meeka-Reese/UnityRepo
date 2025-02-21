using System.Collections;
using UnityEngine;

public class MouseAni : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas canvas;
    
    private bool clicked = false;
    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;
  

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(clickSound, .1f);
        }
        
        

       

        
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera, // Use the camera assigned to the canvas (null if Screen Space - Overlay)
            out mousePos
        );
        rectTransform.anchoredPosition = mousePos;
    }

    
}