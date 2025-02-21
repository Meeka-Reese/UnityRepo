using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    [Header("UI Elements")]
    [SerializeField] private GameObject TextBlock;
    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private GameObject XButton;
    [SerializeField] private Animator animator;
    private Story CurrentStory;
    public bool isPlaying {get; private set;}
    [Header("Dialogue Trigger")]
    [SerializeField] private DialogueTrigger dialogueTrigger;
    
    [Header("Typing Sound")]
    [SerializeField] private AudioClip TypingSound;
    private AudioSource audioSource;
    private bool CorroutineIsRunning = false;
    
    [Header("Choices")]
    [SerializeField] private GameObject[] Choices;
    private TextMeshProUGUI[] ChoicesText;
    private bool ChoiceClicked = false;
    private GameObject Player;
    
    [Header("Nightcore Variable Handling")]
    [SerializeField] private SoundHandler soundHandler;
    private bool NightcorePlaying = false;
    private GameObject DeSat;
    [SerializeField] private SphereGrow sphereGrow;
    [SerializeField] private float TalkingSpeed;


    private void Awake()
    {
        
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of DialogueManager!");
        }
        instance = this;
    }

    private void Start()
    {
        DeSat = GameObject.Find("DeSaturate");
       Setup();
    }

    private void Setup()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        int index = 0;
        ChoicesText = new TextMeshProUGUI[Choices.Length];
        foreach (GameObject choice in Choices)
        {
            ChoicesText[index] = choice.GetComponent<TextMeshProUGUI>();
            choice.transform.parent.gameObject.SetActive(false);
            index++;
        }
        audioSource = GetComponent<AudioSource>();
        XButton.SetActive(false);
        isPlaying = false;
        TextBlock.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
    private void Update()
    {
        if (soundHandler.Night == true && !sphereGrow.SphereKillRunning)
        {
            StartCoroutine(sphereGrow.SphereKill());
        }
        if (!isPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.I) && !CorroutineIsRunning || ChoiceClicked)
        {
            StartCoroutine(ContinueStory());
        }
        float distance = Vector3.Distance(Player.transform.position, transform.position);
        if (distance > 5000)
        {
            ExitDialogue();
        }

        

        
            
        
        
        
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    public void StartDialogue(TextAsset inkJson)
    {
        Setup();
        XButton.SetActive(true);
        animator.SetBool("Talking", true);
    
        // Create a new story instance (this resets progress)
        CurrentStory = new Story(inkJson.text);
        CurrentStory.variablesState["NightcorePlaying"] = soundHandler.Night;
    
        // Explicitly start at the "Main" knot
        // CurrentStory.ChoosePathString("Main"); 
    
        isPlaying = true;
        TextBlock.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    
        // Always start the coroutine, even if it's already running (force restart)
        StopAllCoroutines(); // Ensure no overlapping coroutines
        StartCoroutine(ContinueStory());
    }

    private IEnumerator ContinueStory()
    {
        
        ChoiceClicked = false;
        CorroutineIsRunning = true;
        if (CurrentStory.canContinue)
        {
            DialogueText.GetComponent<TextMeshProUGUI>().text = "";
            string CurrentText = CurrentStory.Continue();
            foreach (char letter in CurrentText.ToCharArray())
            {
                DialogueText.GetComponent<TextMeshProUGUI>().text += letter;
                audioSource.PlayOneShot(TypingSound);
                yield return new WaitForSeconds(0.25f/TalkingSpeed);
            }
            DisplayChoices();
            CorroutineIsRunning = false;
            
        }
        else
        {
            ExitDialogue();
        }
    }

    public void ExitDialogue()
    {
        StopAllCoroutines();
        //kills coroutines so it doesn't keep typing
        XButton.SetActive(false);
        animator.SetBool("Talking", false);
        isPlaying = false;
        TextBlock.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        DialogueText.text = "";
        foreach (GameObject choice in Choices)
        {
            choice.transform.parent.gameObject.SetActive(false);
            choice.gameObject.SetActive(false);
        }
    
        // Reset the coroutine flag
        CorroutineIsRunning = false; // <-- ADD THIS LINE
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = CurrentStory.currentChoices;
        if (currentChoices.Count > Choices.Length)
        {
            Debug.LogWarning("There are more choices to display!");
        }
        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            Choices[index].gameObject.SetActive(true);
            Choices[index].transform.parent.gameObject.SetActive(true);
            ChoicesText[index].text = choice.text;
            index++;
            //loop through ui and choices
        }
        
    }

    public void Choice(int ChoiceIndex)
    {
        
        CurrentStory.ChooseChoiceIndex(ChoiceIndex);
        ChoiceClicked = true;
        List<Choice> currentChoices = CurrentStory.currentChoices;
        
        if (currentChoices.Count < 2)
        {
            foreach (GameObject choice in Choices)
            {
                choice.transform.parent.gameObject.SetActive(false);
                choice.gameObject.SetActive(false);
            }
        }
        if (!CurrentStory.canContinue)
        {
            ExitDialogue();
        }
        
    }
    
   
    
}
