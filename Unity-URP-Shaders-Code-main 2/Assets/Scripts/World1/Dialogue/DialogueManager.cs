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
    

    [SerializeField] private string CharName;
    private AudioSource audioSource;
    private bool CorroutineIsRunning = false;
    private bool BW = false;
    
    [Header("Choices")]
    [SerializeField] private GameObject[] Choices;
    private TextMeshProUGUI[] ChoicesText;
    private bool ChoiceClicked = false;
    private bool ChoiceDisplay = false;
    private GameObject Player;
    
    [Header("Nightcore Variable Handling")]
    [SerializeField] private SoundHandler soundHandler;
    private bool NightcorePlaying = false;
    private GameObject DeSat;
    [SerializeField] private SphereGrow sphereGrow;
    private float TalkingSpeed =10;
    private DialogueVariables dialogueVariables;
    [Header("Screenshot Ask")]
    [SerializeField] private Screenshot screenshot;
    private bool ImageChoice = false;
    private PaintBookPickup paintBookPickup;
    private string SavedState;
    


   
    private void Awake()
    {
        paintBookPickup = FindObjectOfType<PaintBookPickup>();
        
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of DialogueManager!");
        }
        instance = this;
        dialogueVariables = new DialogueVariables();
        
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
        BW = sphereGrow.BW;
        if (soundHandler.Night == true && !sphereGrow.SphereKillRunning)
        {
            StartCoroutine(sphereGrow.SphereKill());
        }
        if (!isPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.I) && !CorroutineIsRunning && !ChoiceDisplay)
        {
            StartCoroutine(ContinueStory());
        }

       TalkingSpeed = (Input.GetKey(KeyCode.I) ? 20 : 10);
        
        float distance = Vector3.Distance(Player.transform.position, transform.position);
        if (distance > 5000)
        {
            ExitDialogue();
        }

        



        ImageChoice = dialogueVariables.ImageChoice;





    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    public void StartDialogue(TextAsset inkJson)
    {
        Setup();
        XButton.SetActive(true);
        if (CharName == "Char")
        {
            animator.SetBool("TalkingChar", true);
            if (BW)
            {
                animator.SetBool("BW", true);
            }
        }
        else if (CharName == "SnowLeopard")
        {
            animator.SetBool("Talking", true);
        }

        // Create a new story instance (this resets progress)
        CurrentStory = new Story(inkJson.text);
        
        CurrentStory.variablesState["NightcorePlaying"] = soundHandler.Night;
    
        // Explicitly start at the "Main" knot
        // CurrentStory.ChoosePathString("Main"); 
    
        isPlaying = true;
        TextBlock.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        dialogueVariables.StartListening(CurrentStory);
        if (SavedState != null)
        {
            CurrentStory.state.LoadJson(SavedState);
            
        }
        CurrentStory.ChoosePathString("InitialChecks");

        // Always start the coroutine, even if it's already running (force restart)
        StopAllCoroutines(); // Ensure no overlapping coroutines
        StartCoroutine(ContinueStory());
    }

    private IEnumerator ContinueStory()
    {
        
        
        CurrentStory.variablesState["PaintTab"] = paintBookPickup.BookPickedUp;
        
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
            SavedState = CurrentStory.state.ToJson();

            if (CurrentStory.currentChoices.Count > 0)
            {
                DisplayChoices();
            }

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
        animator.SetBool("BW", false);
        animator.SetBool("TalkingChar", false);
        isPlaying = false;
        dialogueVariables.StopListening(CurrentStory);
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
        ChoiceDisplay = true;
        List<Choice> currentChoices = CurrentStory.currentChoices;
        if (currentChoices.Count > Choices.Length)
        {
            Debug.LogWarning("There are more choices to display!");
        }
        int index = 0;
        if (!ImageChoice)
        {
            foreach (Choice choice in currentChoices)
            {
                Choices[index].gameObject.SetActive(true);
                Choices[index].transform.parent.gameObject.SetActive(true);
                ChoicesText[index].text = choice.text;
                index++;
                //loop through ui and choices
            }
        }
        else if (ImageChoice)
        {
            foreach (Choice choice in currentChoices)
            {
                int Index = 0;
                if (Index < screenshot.screenshotCount)
                {
                    Choices[index].gameObject.SetActive(true);
                    Choices[index].transform.parent.gameObject.SetActive(true);
                    ChoicesText[index].text = choice.text;
                    index++;
                }
                //loop through ui and choices
            }
        }

    }

    public void Choice(int ChoiceIndex)
    {
        
        CurrentStory.ChooseChoiceIndex(ChoiceIndex);
        ChoiceClicked = true;
        ChoiceDisplay = false;
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
        StartCoroutine(ContinueStory());
        
    }
    
   
    
}
