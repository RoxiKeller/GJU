using UnityEngine;

public class SuspicionSystem : MonoBehaviour
{
    public King king;

    public static SuspicionSystem Instance;

    [Header("Suspicion")]
    public float suspicion = 0f;
    public float maxSuspicion = 100f;

    public System.Action<float> OnSuspicionChanged;

    [Header("Difficulty Scaling")]
    public AnimationCurve difficultyCurve = AnimationCurve.Linear(0, 1, 1, 2);
    public float currentDifficultyMult = 1f;

    // Inside SuspicionSystem.cs
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // 1. Handle Difficulty Scaling based on Time
        // Ensure you have a reference to your Timer script!
        if (Timer.Instance != null)
        {
            float t = 1f - (Timer.Instance.timeRemaining / Timer.Instance.totalLevelTime);
            currentDifficultyMult = difficultyCurve.Evaluate(t);
        }

        // 2. Optional: Passive Decay (Reduces suspicion if the player is doing well)
        if (suspicion > 0 && !AnyActiveThreats())
        {
            suspicion -= Time.deltaTime * 1.0f; // Slower than the gains
        }

        // 3. Clamp value so it doesn't go below 0 or above 100
        suspicion = Mathf.Clamp(suspicion, 0, maxSuspicion);

        if (suspicion >= maxSuspicion && !isGameOver)
        {
            isGameOver = true;
            LoseUI.Instance.ShowLoseScreen();
        }
    }

    private bool AnyActiveThreats()
    {
        // 1. Check Wind
        if (Wind.Instance != null && Wind.Instance.IsWindActive()) return true;

        // 2. Check Dog
        Dog dog = Object.FindAnyObjectByType<Dog>();
        if (dog != null && dog.IsMovingToOrInspectingKing()) return true;

        // 3. Check Storytellers
        Storyteller st = Object.FindAnyObjectByType<Storyteller>();
        if (st != null && st.IsInspectingKing()) return true; 

        // 4. Check the "Blink-Watching" Villagers
        // We search for the base NPC class because any NPC 
        // currently "Inspecting" is a threat to the secret.
        NPC[] allNPCs = Object.FindObjectsByType<NPC>(FindObjectsSortMode.None);
        foreach (NPC npc in allNPCs)
        {
            // If any NPC is currently at the King (and isn't the Dog/Storyteller already checked)
            if (npc.IsInspectingKing()) return true;
        }

        return false;
    }

    public void AddSuspicion(float amount)
    {
        suspicion += amount;
        suspicion = Mathf.Clamp(suspicion, 0f, maxSuspicion);

        OnSuspicionChanged?.Invoke(suspicion);

        // Trigger the UI popup
        if (SuspicionFeedback.Instance != null) 
            SuspicionFeedback.Instance.ShowChange(amount);

        if (suspicion >= maxSuspicion)
        {
            GameOver();
        }
    }

    public void ReduceSuspicion(float amount)
    {
        suspicion -= amount;
        suspicion = Mathf.Clamp(suspicion, 0f, maxSuspicion);

        OnSuspicionChanged?.Invoke(suspicion);

        // Trigger the UI popup (passing as negative)
        if (SuspicionFeedback.Instance != null) 
            SuspicionFeedback.Instance.ShowChange(-amount);
    }

    void GameOver()
    {
        Debug.Log("👑 THE LIE HAS BEEN EXPOSED");

        if (king != null)
        {
            SpriteRenderer sr = king.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = Color.red;
        }

        // Call the Singleton directly
        if (LoseUI.Instance != null)
        {
            LoseUI.Instance.ShowLoseScreen();
        }
        else
        {
            Debug.LogError("LoseUI Instance is null! Is the script on an object in the scene?");
        }
    }
}