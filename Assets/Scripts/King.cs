using UnityEngine;

public class King : MonoBehaviour
{
    [Header("Stability")]
    public float stability = 100f;
    public float maxStability = 100f;

    [Header("Visual Tilt")]
    public float maxTilt = 15f;
    public float tiltRecoverySpeed = 2f;

    private float currentTilt;
    public WindSkillCheck skillCheck;
    private bool isChecking = false;

    void OnEnable()
    {
        Wind.OnWindHit += ApplyWind;
    }

    void OnDisable()
    {
        Wind.OnWindHit -= ApplyWind;
    }

    void Update()
    {
        RecoverStability();
        ApplyTilt();
    }

    void ApplyWind(float force)
    {
        stability -= force;

        currentTilt += Random.Range(-maxTilt, maxTilt);

        stability = Mathf.Clamp(stability, 0f, maxStability);

        // 🚨 ONLY TRIGGER IF NOT ALREADY ACTIVE
        if (!isChecking && skillCheck != null)
        {
            isChecking = true;

            skillCheck.gameObject.SetActive(true);
            skillCheck.StartCheck();

            skillCheck.OnCheckFinished = ResetCheckLock;
        }
    }

    void ResetCheckLock()
    {
        isChecking = false;
    }

    void RecoverStability()
    {
        stability += Time.deltaTime * 5f;
        stability = Mathf.Clamp(stability, 0f, maxStability);

        currentTilt = Mathf.Lerp(currentTilt, 0f, Time.deltaTime * tiltRecoverySpeed);
    }

    void ApplyTilt()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, currentTilt);
    }

    public bool IsCollapsed()
    {
        return stability <= 0f;
    }
}