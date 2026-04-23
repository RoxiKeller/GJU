using UnityEngine;

public class SuspicionSystem : MonoBehaviour
{
    public King king;

    public static SuspicionSystem Instance;

    [Header("Suspicion")]
    public float suspicion = 0f;
    public float maxSuspicion = 100f;

    public System.Action<float> OnSuspicionChanged;

    void Awake()
    {
        Instance = this;
    }

    public void AddSuspicion(float amount)
    {
        suspicion += amount;
        suspicion = Mathf.Clamp(suspicion, 0f, maxSuspicion);

        OnSuspicionChanged?.Invoke(suspicion);

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
    }

    void GameOver()
    {
        // Make the King's sprite fall over or turn red
        king.GetComponent<SpriteRenderer>().color = Color.red;
        
        // Then show the UI
        Object.FindAnyObjectByType<LoseUI>().ShowLoseScreen();
    }
}