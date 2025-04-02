using System.Collections;
using TMPro;
using UnityEngine;

public class TextFlash : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    [SerializeField] float FlashDuration = 0.2f;
    private bool CoIsRunning = false;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!CoIsRunning)
        {
            StartCoroutine(FlashText());
        }
    }

    private IEnumerator FlashText()
    {
        CoIsRunning = true;
        
        
        while (true)
        {
            textMesh.color = Color.black;
            yield return new WaitForSeconds(FlashDuration);
            textMesh.color = Color.clear;
            yield return new WaitForSeconds(FlashDuration);
            Debug.Log("Flash");
        }

    }
}
