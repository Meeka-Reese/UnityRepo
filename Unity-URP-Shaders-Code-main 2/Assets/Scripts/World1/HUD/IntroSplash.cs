using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroSplash : MonoBehaviour
{
    private RawImage image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<RawImage>();
        StartCoroutine(Splash());
    }

    private IEnumerator Splash()
    {
        float Alpha = 0;
        while (Alpha < 1)
        {
            Alpha = Mathf.MoveTowards(Alpha, 1f, Time.deltaTime *  1.5f);
            image.color = new Color(image.color.r, image.color.g, image.color.a, Alpha);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        while (Alpha > 0)
        {
            Alpha = Mathf.MoveTowards(Alpha, 0f, Time.deltaTime);
            image.color = new Color(image.color.r, image.color.g, image.color.b,Alpha);
            yield return null;
        }
    }
    
}
