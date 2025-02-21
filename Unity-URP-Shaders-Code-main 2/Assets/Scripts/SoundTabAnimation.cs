using UnityEngine;
using UnityEngine.UI;

public class SoundTabAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    private SoundDrag soundDrag;
    private bool minimized = false;
    
    private Image image;

    private Color Blank = new Color(0, 0, 0, 0);
    private Color White = new Color(1, 1, 1, 1);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        soundDrag = FindObjectOfType<SoundDrag>();
        image = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        minimized = soundDrag._minimize;
        if (!minimized)
        {
            image.color = Blank;
        }
        else
        {
            image.color = White;
        }
        animator.SetBool("Minimized", minimized);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Minimize 1") && 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            minimized = false;
            image.color = Blank;
        }
    }
    
}