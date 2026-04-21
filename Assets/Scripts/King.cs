using UnityEngine;

public class King : MonoBehaviour
{
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

    public NPC npc;

    void OnEnable() { Wind.OnWindHit += ApplyWind; }
    void OnDisable() { Wind.OnWindHit -= ApplyWind; }

    void Update()
    {
        blinkTimer += Time.deltaTime;
        RecoverStability();
        ApplyTilt();
    }

    public void OnBlinkButton() // Call this from your UI Button's OnClick()
    {
        float timeSinceLast = Time.time - lastBlinkTime;

        // 1. Detect Spamming
        if (timeSinceLast < 0.3f) 
        {
            // If they mash the button, notify the NPC immediately
            if (npc != null) npc.StartInspection("His eyes are twitching!");
            SuspicionSystem.Instance.AddSuspicion(10f);
        }

        // 2. Trigger the actual blink
        Blink(); 
    }

    public void Blink()
    {
        // Calculate how long it's been since the last blink
        float timeSinceLastBlink = Time.time - lastBlinkTime;

        // If the interval is too short, the king looks glitchy/suspicious
        if (timeSinceLastBlink < safeBlinkInterval)
        {
            float penaltyMultiplier = 1f / (timeSinceLastBlink + 0.1f); // Higher penalty for faster mashing
            SuspicionSystem.Instance.AddSuspicion(spamPenalty * penaltyMultiplier * Time.deltaTime);
            Debug.Log("Blinking too fast! Suspicion rising.");
        }

        blinkTimer = 0f;
        lastBlinkTime = Time.time;

        // TELL THE NPC WE BLINKED
        if (npc != null)
        {
            npc.OnKingBlink();
        }

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