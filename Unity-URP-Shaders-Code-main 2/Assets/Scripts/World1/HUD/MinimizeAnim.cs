using UnityEngine;

public class MinimizeAnim : MonoBehaviour
{
    [SerializeField] Animator animator;
    private MouseDragScript mouseDragScript;
    private bool minimized = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mouseDragScript = FindObjectOfType<MouseDragScript>();
        
    }

    // Update is called once per frame
    void Update()
    {
        minimized = mouseDragScript._minimize;
        
            
            animator.SetBool("Minimized", minimized);
        }
    
    }

