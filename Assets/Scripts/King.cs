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

    [Header("Blink Settings")]
    public float safeBlinkInterval = 2.0f; 
    public float lastBlinkTime;
    public float blinkTimer = 0f;

    private float currentTilt;
    public WindSkillCheck skillCheck;
    private bool isChecking = false;

    [Header("Animations")]
    public Animator kingAnimator;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip blinkSound;

    void OnEnable() { Wind.OnWindHit += ApplyWind; }
    void OnDisable() { Wind.OnWindHit -= ApplyWind; }

    void Start()
    {
        // Set the last blink to 2 seconds (or more) BEFORE the game started.
        // This makes the very first blink "safe."
        lastBlinkTime = -safeBlinkInterval;
    }

    void Update()
    {
        blinkTimer += Time.deltaTime;
        RecoverStability();
        ApplyTilt();
    }

    public void OnBlinkButton() 
    {
        // 1. Play the Sound
        if (audioSource != null && blinkSound != null)
        {
            // Randomize pitch slightly (e.g., between 0.9 and 1.1)
            AudioManager.instance.PlaySound(AudioManager.instance.Blink);
        }

        // 2. Tell the Animator to play the blink
        if (kingAnimator != null)
        {
            kingAnimator.SetTrigger("BlinkTrigger");
        }

        // 3. Logic and Suspicion
        BlinkLogic(); 
    }

    private void BlinkLogic()
    {
        OnBlinkEvent?.Invoke();

        float timeSinceLastBlink = Time.time - lastBlinkTime;

        if (timeSinceLastBlink < 0.3f) 
        {
            // PANIC BLINK
            SuspicionSystem.Instance.AddSuspicion(2f);
        }
        else if (timeSinceLastBlink < safeBlinkInterval)
        {
            // TOO SOON
            SuspicionSystem.Instance.AddSuspicion(2f);
        }
        else 
        {
            // PERFECTLY CALM
            SuspicionSystem.Instance.ReduceSuspicion(1f); 
        }

        lastBlinkTime = Time.time;
        blinkTimer = 0f;
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