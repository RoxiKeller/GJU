using UnityEngine;

public class King : MonoBehaviour
{
    public System.Action OnBlinkEvent;

    [Header("Stability")]
    public float stability = 100f;
    public float maxStability = 100f;

    [Header("Visual Tilt")]
    public float maxTilt = 15f;
    public float tiltRecoverySpeed = 2f;

    [Header("Blink Suspicion")]
    [Tooltip("How fast the player can blink naturally (seconds)")]
    public float safeBlinkInterval = 2.0f; 
    [Tooltip("Suspicion added if blinking too fast")]
    public float spamPenalty = 10f;
    public float lastBlinkTime;

    private float currentTilt;
    public WindSkillCheck skillCheck;
    private bool isChecking = false;

    public SpriteRenderer sr;
    public float blinkDuration = 0.1f;
    public float blinkScale = 0.9f;
    public float blinkTimer = 0f;

    void OnEnable() { Wind.OnWindHit += ApplyWind; }
    void OnDisable() { Wind.OnWindHit -= ApplyWind; }

    void Update()
    {
        blinkTimer += Time.deltaTime;
        RecoverStability();
        ApplyTilt();
    }

    public void OnBlinkButton() 
    {
        float timeSinceLast = Time.time - lastBlinkTime;

        if (timeSinceLast < 0.3f) 
        {
            // Just let the NPC comment, don't punish the bar as much
            SuspicionSystem.Instance.AddSuspicion(2f); // Lowered from 10f
        }

        Blink(); 
    }

    public void Blink()
    {
        OnBlinkEvent?.Invoke();

        float timeSinceLastBlink = Time.time - lastBlinkTime;

        // Only add a tiny bit of suspicion if they blink faster than the safe interval
        if (timeSinceLastBlink < safeBlinkInterval)
        {
            // Use a flat, small value instead of a multiplier
            // Example: adds just 1% or 2% suspicion per "fast" blink
            float gentlePenalty = 2f; 
            
            if (SuspicionSystem.Instance != null)
            {
                SuspicionSystem.Instance.AddSuspicion(gentlePenalty);
            }
        }

        // Reset timers and visuals
        blinkTimer = 0f;
        lastBlinkTime = Time.time;

        StopAllCoroutines();
        StartCoroutine(BlinkRoutine());
    }

    System.Collections.IEnumerator BlinkRoutine()
    {
        Vector3 originalScale = transform.localScale;
        Color originalColor = sr.color;

        transform.localScale = originalScale * blinkScale;
        sr.color = Color.gray;

        yield return new WaitForSeconds(blinkDuration);

        transform.localScale = originalScale;
        sr.color = originalColor;
    }

    void ApplyWind(float force)
    {
        stability -= force;
        currentTilt += Random.Range(-maxTilt, maxTilt);
        stability = Mathf.Clamp(stability, 0f, maxStability);

        if (!isChecking && skillCheck != null)
        {
            isChecking = true;
            skillCheck.gameObject.SetActive(true);
            skillCheck.StartCheck();
            skillCheck.OnCheckFinished = ResetCheckLock;
        }
    }

    void ResetCheckLock() => isChecking = false;

    void RecoverStability()
    {
        stability += Time.deltaTime * 5f;
        stability = Mathf.Clamp(stability, 0f, maxStability);
        currentTilt = Mathf.Lerp(currentTilt, 0f, Time.deltaTime * tiltRecoverySpeed);
    }

    void ApplyTilt() => transform.rotation = Quaternion.Euler(0f, 0f, currentTilt);
}