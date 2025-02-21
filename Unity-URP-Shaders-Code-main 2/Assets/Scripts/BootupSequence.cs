using UnityEngine;

public class BootupSequence : MonoBehaviour
{
    
    private AudioSource audioSource;
    [SerializeField] private AudioClip BootClip;
    [SerializeField] private Animator animator;
    private GameObject HomeHUD;
    void Start()
    {
        HomeHUD = GameObject.Find("HomeHUD");
        HomeHUD.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(BootClip);
    }

  
    void Update()
    {
        
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Check if animation has finished playing
            if (stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0))
            {
                HomeHUD.SetActive(true);
            }


    }
}
