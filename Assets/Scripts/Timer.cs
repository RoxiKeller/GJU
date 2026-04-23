using UnityEngine;
using TMPro; // Required for the UI
using UnityEngine.SceneManagement; // To restart or go to a Win Screen

public class Timer : MonoBehaviour
{
    [Header("Time Settings")]
    public float timeRemaining = 120f; // 2 minutes to survive
    private bool timerIsRunning = false;

    [Header("UI Elements")]
    public TextMeshProUGUI timeText;
    public GameObject winPanel; // A UI panel that says "You Survived!"

    void Start()
    {
        timerIsRunning = true;
        if (winPanel != null) winPanel.SetActive(false);
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
                WinGame();
            }
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