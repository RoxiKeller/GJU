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

    private float timer;

    // Event: sends wind force to anything listening
    public static Action<float> OnWindHit;

    public bool isSkillCheckActive = false;

    void Awake()
    {
        Instance = this;
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
                // During skill check: keep pressure high
                timer = UnityEngine.Random.Range(0.8f, 2f);
            }
            else
            {
                // Normal pacing
                SetNextWind();
            }
        }
    }

    public void OnSkillCheckStart()
    {
        isSkillCheckActive = true;
    }

    public void OnSkillCheckEnd()
    {
        isSkillCheckActive = false;
        SetNextWind(); // restore normal pacing cleanly
    }

    void TriggerWind()
    {
        float force = UnityEngine.Random.Range(minForce, maxForce);

        Debug.Log("🌬 Wind triggered: " + force);

        OnWindHit?.Invoke(force);
    }

    void SetNextWind()
    {
        timer = UnityEngine.Random.Range(minInterval, maxInterval);
    }
}