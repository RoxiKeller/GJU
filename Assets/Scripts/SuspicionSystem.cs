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