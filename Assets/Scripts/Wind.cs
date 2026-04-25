using System;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public static Wind Instance;

    [Header("Wind Settings")]
    public float minInterval = 3f;
    public float maxInterval = 8f;

    public float minForce = 10f;
    public float maxForce = 40f;

    [Header("Wind Visual")]
    public GameObject windVisual; // assign animated sprite child here

    private float timer;

    // Event: sends wind force to anything listening
    public static Action<float> OnWindHit;

    public bool isSkillCheckActive = false;

    void Awake()
    {
        Instance = this;

        if (windVisual != null)
            windVisual.SetActive(false);
    }

    void Start()
    {
        SetNextWind();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            TriggerWind();

            if (isSkillCheckActive)
            {
                // while player is handling the skill check,
                // keep wind pressure active with shorter gusts
                timer = UnityEngine.Random.Range(0.8f, 2f);
            }
            else
            {
                SetNextWind();
            }
        }
    }

    public void OnSkillCheckStart()
    {
        isSkillCheckActive = true;

        // 🌬 keep wind visible during the whole skill check
        if (windVisual != null)
            windVisual.SetActive(true);
    }

    public void OnSkillCheckEnd()
    {
        isSkillCheckActive = false;

        // 🌬 hide wind only after the player finishes
        if (windVisual != null)
            windVisual.SetActive(false);

        SetNextWind(); // restore normal pacing
    }

    void TriggerWind()
    {
        float force = UnityEngine.Random.Range(minForce, maxForce);
        if (AudioManager.instance != null)
        AudioManager.instance.PlaySound(AudioManager.instance.Wind);
        Debug.Log("🌬 Wind triggered: " + force);

        // make sure visual appears immediately
        if (windVisual != null)
            windVisual.SetActive(true);

        OnWindHit?.Invoke(force);
    }

    void SetNextWind()
    {
        float mult = SuspicionSystem.Instance.currentDifficultyMult;
        timer = UnityEngine.Random.Range(minInterval, maxInterval) / mult;
    }

    // Add this inside the Wind class
    public bool IsWindActive()
    {
        // The wind is a "threat" as long as the skill check is on screen
        return isSkillCheckActive;
    }
}