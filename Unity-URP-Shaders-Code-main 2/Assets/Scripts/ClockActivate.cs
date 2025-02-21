using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ClockActivate : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private Animator CursorLoadAnimator;
    private GameObject Clock;
    private GameObject Button;
    private GameObject Notes;
    private GameObject Gallery;
    

    void Start()
    {
        
        
        Clock = GameObject.Find("Time");
        Button = GameObject.Find("Planet1");
        Notes = GameObject.Find("Notes");
        Gallery = GameObject.Find("Gallery");
        
        
    }

    
    void Update()
    {
            
            bool AnimatorIsPlaying = animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 || animator.IsInTransition(0);
            if (!AnimatorIsPlaying)
            {
                Clock.SetActive(true);
                Button.SetActive(true);
                Notes.SetActive(true);
                Gallery.SetActive(true);
            }

            if (AnimatorIsPlaying)
            {
                Clock.SetActive(false);
                Button.SetActive(false);
                Notes.SetActive(false);
                Gallery.SetActive(false);
                
            }

            
        
    }

    

    public void LoadLevelBtn(int SceneIndex)
    {
         StartCoroutine(LoadLevelAsync(SceneIndex));
    }

    IEnumerator LoadLevelAsync(int SceneIndex)
    {
        CursorLoadAnimator.SetBool("Loading", true);
        yield return new WaitForSeconds(3f);
        Application.backgroundLoadingPriority = ThreadPriority.High;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneIndex);
        
        yield return null;
    }
}