using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Talking : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private GameObject SpeakerObject;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject XButton;
    [SerializeField] private Camera MainCamera;
    [SerializeField] private float MaxTalkDistance = 3000;
    private GameObject TextBlock;
    [SerializeField] private GameObject DialogueText;
    private AudioSource audioSource;
    [SerializeField] private AudioClip TextType;

    [SerializeField] private string[] TalkingText;
    [SerializeField] private string FinishedTalking;
    [SerializeField] private string TagName;
    private bool IsTalking = false;
    private int TalkingIndex = 0;
    private bool Closed = false;
    private bool DonePrintingText = true;

    [SerializeField] private LayerMask ignoreLayer1;
    [SerializeField] private LayerMask ignoreLayer2;
    private LayerMask combinedIgnoreLayers;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        TextBlock = GameObject.Find("SnowText");
        TextBlock.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        XButton.SetActive(false);
        SpeakerObject = GameObject.Find("SpeakerL");
        combinedIgnoreLayers = ignoreLayer1 | ignoreLayer2;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = MainCamera.ScreenPointToRay(Input.mousePosition);

            // Perform raycast and ignore objects on the "Notification" layer
            Debug.Log(DonePrintingText);
            if (Physics.Raycast(ray, out hit, MaxTalkDistance, ~combinedIgnoreLayers) && DonePrintingText)
            {
                if (hit.collider != null && hit.collider.gameObject.CompareTag(TagName))
                {
                    Closed = false;
                    // You hit a Speaker object, perform actions
                    animator.SetBool("Talking", true);
                    XButton.SetActive(true);
                    DialogueText.GetComponent<TextMeshProUGUI>().text = "";
                    StartCoroutine(ShortWait());
                }
                
            }
        }
    }

    public void Close()
    {
        Closed = true;
        animator.SetBool("Talking", false);
        XButton.SetActive(false);
        TextBlock.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        DialogueText.SetActive(false);
    }
    

    private IEnumerator ShortWait()
    {
        
            DonePrintingText = false;
            DialogueText.GetComponent<TextMeshProUGUI>().text = "";
            yield return new WaitForSeconds(.2f);
            TextBlock.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            DialogueText.SetActive(true);
            if (TalkingIndex < TalkingText.Length)
            {
                StartCoroutine(TypeLine(TalkingText[TalkingIndex]));
            }
            else
            {
                StartCoroutine(TypeLine(FinishedTalking));
            }

            TalkingIndex++;
            



    }

    

    private IEnumerator TypeLine(string Text)
    {
        bool TypeAudio = true;
        foreach (char letter in Text.ToCharArray())
        {
            if (!Closed)
            {
                TypeAudio = !TypeAudio;
                DialogueText.GetComponent<TextMeshProUGUI>().text += letter;
                if (TypeAudio)
                {
                    audioSource.PlayOneShot(TextType);
                }
                yield return new WaitForSeconds(.04f);
            }

            if (Closed)
            {
                break;
            }
        }
        DonePrintingText = true;
    }
}