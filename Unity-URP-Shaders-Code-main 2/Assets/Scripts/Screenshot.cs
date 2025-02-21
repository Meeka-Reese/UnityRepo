using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Screenshot : MonoBehaviour
{
    [SerializeField] RectTransform targetUIObject; // Target UI object to capture
    [SerializeField] RenderTexture renderTexture; // Render texture for capturing
    [SerializeField] MouseDragScript mouseDragScript;
    private GameObject[] screenshotSlots;
    private GameObject SoundTab;
    private GameObject MouseCursor;
    private int slotIndex = 0;
    private NumberReader numberReader;

    private void Start()
    {
        SoundTab = GameObject.Find("SoundTab");
        MouseCursor = GameObject.Find("MouseCursor");
        numberReader = GameObject.Find("SentisAIManager").GetComponent<NumberReader>();
        int i = 0;
        int i2 = 1;
        // Find all slots tagged with "Slot"
        screenshotSlots = GameObject.FindGameObjectsWithTag("Slot");
        foreach (GameObject slot in screenshotSlots)
        {
            screenshotSlots[i] = GameObject.Find("Slot" + i2);
           
            
            i++;
            i2++;
        }

        if (screenshotSlots.Length == 0)
        {
            Debug.LogError("No screenshot slots found! Make sure slots are tagged correctly.");
        }
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && !mouseDragScript._minimize)
        {
            StartCoroutine(TakeScreenshot());
        }
    }

    private void ShiftSlots()
    {
        // Shift sprites in reverse order
        for (int i = screenshotSlots.Length - 1; i > 0; i--)
        {
            screenshotSlots[i].GetComponent<Image>().sprite = screenshotSlots[i - 1].GetComponent<Image>().sprite;
        }
    }

    private IEnumerator TakeScreenshot()
    {
        bool SoundState = SoundTab.activeSelf;
        SoundTab.SetActive(false);
        MouseCursor.SetActive(false);
        RenderTexture.active = renderTexture;

        yield return new WaitForEndOfFrame();

        // Calculate the screen-space rectangle of the target UI object
        Vector3[] worldCorners = new Vector3[4];
        targetUIObject.GetWorldCorners(worldCorners);

        // Convert world corners to screen space
        Vector2 minScreenPoint = RectTransformUtility.WorldToScreenPoint(null, worldCorners[0]);
        Vector2 maxScreenPoint = RectTransformUtility.WorldToScreenPoint(null, worldCorners[2]);

        int width = Mathf.RoundToInt(maxScreenPoint.x - minScreenPoint.x);
        int height = Mathf.RoundToInt(maxScreenPoint.y - minScreenPoint.y);
        int x = Mathf.RoundToInt(minScreenPoint.x);
        int y = Mathf.RoundToInt(minScreenPoint.y);

        // Create and apply a new texture
        
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        screenshotTexture.ReadPixels(new Rect(x, y, width, height), 0, 0);
        screenshotTexture.Apply();
        numberReader.Execute(screenshotTexture);

        // Create a sprite from the texture
        Sprite screenshotSprite = Sprite.Create(screenshotTexture, new Rect(0, 0, screenshotTexture.width, screenshotTexture.height), new Vector2(0.5f, 0.5f));
        SoundTab.SetActive(SoundState);
        MouseCursor.SetActive(true);
        // Shift existing screenshots
        ShiftSlots();

        
            screenshotSlots[0].GetComponent<Image>().sprite = screenshotSprite;
        

        RenderTexture.active = null;
    }
}
