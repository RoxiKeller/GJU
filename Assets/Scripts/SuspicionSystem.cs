using UnityEngine;

public class SuspicionSystem : MonoBehaviour
{
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
        Debug.Log("👑 THE LIE HAS BEEN EXPOSED");
        // trigger scene reset / death screen
    }
}