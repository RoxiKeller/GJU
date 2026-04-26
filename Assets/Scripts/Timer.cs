using UnityEngine;
using TMPro; // Required for the UI
using UnityEngine.SceneManagement; // To restart or go to a Win Screen

public class Timer : MonoBehaviour
{
    public static Timer Instance; 
    [Header("Time Settings")]
    public float timeRemaining = 120f; // 2 minutes to survive
    private bool timerIsRunning = false;

    [Header("UI Elements")]
    public TextMeshProUGUI timeText;
    public GameObject winPanel; // A UI panel that says "You Survived!"

    [Header("Progression Settings")]
    public float totalLevelTime = 120f; // 2 minutes

    // Thresholds (Seconds remaining when these start)
    // Order: 1. Blink (Default) -> 2. Dog -> 3. Storytellers -> 4. Wind
    public float dogStartTime = 110f;         // 10 seconds in
    public float storytellerStartTime = 80f;  // 40 seconds in
    public float windStartTime = 50f;         // 1 minute 10 seconds in

    [Header("References to Toggle")]
    public Wind windScript;
    public Dog dogScript; // Or whatever your Dog script is named
    public Storyteller storytellerScript;

    private bool isGameWon = false; // Add this flag at the top
    private bool isIntroAnnounced = false;
    private float helpTimer = 0f;
    private bool isHecticPhase = false; // Add this variable at the top

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timerIsRunning = true;
        if (winPanel != null) winPanel.SetActive(false);
        
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);

                if (timeRemaining < 30f && !isHecticPhase)
                {
                    TriggerHecticPhase();
                }
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;

                // ONLY call WinGame if we haven't already
                if (!isGameWon)
                {
                    isGameWon = true; 
                    WinGame();
                }
            }
        }

        HandleProgression();
    }

    

    void HandleProgression()
    {
        if (timeRemaining <= totalLevelTime && !isIntroAnnounced)
    {
        isIntroAnnounced = true; // Set to true so it never runs again
        AnnouncementUI.Instance.Display("The King is dead. Don't let them find out. ACT NORMAL.");
    }
        // 1. DOG (First New Threat)
        if (timeRemaining <= dogStartTime && dogScript != null && !dogScript.enabled)
        {
            dogScript.enabled = true; 
            AnnouncementUI.Instance.Display("Your dog is blowing your cover! Distract it!");
        }

        // 2. STORYTELLERS (Second Threat)
        if (timeRemaining <= storytellerStartTime && storytellerScript != null && !storytellerScript.enabled)
        {
            storytellerScript.enabled = true;
            AnnouncementUI.Instance.Display("Peasants are approaching! Drag mask to king to equip, left click king to unequip");
        }

        // 3. WIND (Final Major Threat)
        if (timeRemaining <= windStartTime && windScript != null && !windScript.enabled)
        {
            windScript.enabled = true;
            AnnouncementUI.Instance.Display("The wind is blowing! Use SPACE to keep the King upright!");
        }
    }

    void TriggerHecticPhase()
    {
        isHecticPhase = true;
        timeText.color = Color.red;
    }
    void DisplayTime(float timeToDisplay)
    {
        // Formats the time as Minutes:Seconds (e.g., 01:45)
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void WinGame()
    {
        Debug.Log("LEVEL COMPLETE");

        // 1. Trigger the Visual UI via the WinUI Singleton
        if (WinUI.Instance != null)
        {
            WinUI.Instance.ShowWinScreen();
        }

        // 2. Cleanup gameplay objects
        Object.FindAnyObjectByType<NPC>()?.gameObject.SetActive(false);
        if (Wind.Instance != null) Wind.Instance.gameObject.SetActive(false);
        
        // Play the alarm sting once
       // AudioManager.instance.PlaySound(AudioManager.instance.Alarm);
    }
}