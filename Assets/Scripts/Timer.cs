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

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timerIsRunning = true;
        if (winPanel != null) winPanel.SetActive(false);
        AudioManager.instance.PlaySound(AudioManager.instance.CK_intro);
    }

    private bool isHecticPhase = false; // Add this variable at the top

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                
                // Check for the 30-second mark
                if (timeRemaining < 30f && !isHecticPhase)
                {
                    TriggerHecticPhase();
                }
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                AudioManager.instance.PlaySound(AudioManager.instance.Alarm);
                WinGame();
            }
        }

        HandleProgression();
    }

    void HandleProgression()
    {
        // 1. DOG (First New Threat)
        if (timeRemaining <= dogStartTime && dogScript != null && !dogScript.enabled)
        {
            dogScript.enabled = true; 
            AnnouncementUI.Instance.Display("Your dog is blowing your cover! Distract him!");
        }

        // 2. STORYTELLERS (Second Threat)
        if (timeRemaining <= storytellerStartTime && storytellerScript != null && !storytellerScript.enabled)
        {
            storytellerScript.enabled = true;
            AnnouncementUI.Instance.Display("The peasants want to talk to the king! Drag mask to king to equip, left click to unequip");
        }

        // 3. WIND (Final Major Threat)
        if (timeRemaining <= windStartTime && windScript != null && !windScript.enabled)
        {
            windScript.enabled = true;
            AnnouncementUI.Instance.Display("The wind is tilting the cardboard king! Resist by trying to hit the green zone with Space!");
        }
    }

    void TriggerHecticPhase()
    {
        isHecticPhase = true;
        timeText.color = Color.red;
        
        // Set wind to be super aggressive once
        if (Wind.Instance != null)
        {
            Wind.Instance.minInterval = 1f;
            Wind.Instance.maxInterval = 3f;
            Debug.Log("FINAL STRETCH: Wind intensity increased!");
        }
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
        // Stop all NPCs and gameplay logic
        if (winPanel != null) winPanel.SetActive(true);
        
        // Disable the Dog, Storytellers, and Wind to let the player relax
        Object.FindAnyObjectByType<NPC>()?.gameObject.SetActive(false);
        Wind.Instance.gameObject.SetActive(false);

        Debug.Log("LEVEL COMPLETE");
    }
}