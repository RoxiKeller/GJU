using UnityEngine;

public class WindSkillCheck : MonoBehaviour
{
    public System.Action OnCheckFinished;

    [Header("Random Zone")]
    public float zonePadding = 50f;

    [Header("UI Elements")]
    public RectTransform pointer;
    public RectTransform playArea;

    [Header("Zones")]
    public RectTransform perfectZone;

    [Header("Movement")]
    public float speed = 2f;
    private bool movingRight = true;

    [Header("Result")]
    public bool isActive = false;

    [Header("Time Pressure")]
    public float timeToStartPenalty = 1.5f;
    public float suspicionPerSecond = 5f;

    private float timer;
    private bool penaltyActive;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isActive) return;

        MovePointer();

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            penaltyActive = true;
        }

        if (penaltyActive)
        {
            SuspicionSystem.Instance.AddSuspicion(suspicionPerSecond * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckResult();
        }
    }

    void RandomizePerfectZone()
    {
        float areaHalfWidth = playArea.rect.width / 2f;
        
        // --- NEW DYNAMIC SCALING ---
        float mult = SuspicionSystem.Instance.currentDifficultyMult;

        // Shrink the zone: At mult 1.0, scale is 1.0. At mult 2.0, scale is 0.5.
        // We use 1f / mult to ensure it gets smaller as difficulty goes up.
        float scaleFactor = Mathf.Clamp(1f / mult, 0.4f, 1f); 
        perfectZone.localScale = new Vector3(scaleFactor, 1, 1);

        // After scaling, we must re-calculate half width for positioning
        float zoneHalfWidth = (perfectZone.rect.width * scaleFactor) / 2f;

        float minX = -areaHalfWidth + zoneHalfWidth;
        float maxX = areaHalfWidth - zoneHalfWidth;
        float randomX = Random.Range(minX, maxX);

        Vector2 pos = perfectZone.anchoredPosition;
        pos.x = randomX;
        perfectZone.anchoredPosition = pos;
    }

    void MovePointer()
    {
        // --- NEW DYNAMIC SPEED ---
        float mult = SuspicionSystem.Instance.currentDifficultyMult;
        
        // Base speed is multiplied by the difficulty
        // If base speed is 2, at peak difficulty it becomes 4 or more.
        float dynamicSpeed = speed * mult;

        float step = dynamicSpeed * Time.deltaTime * 200f;

        Vector2 pos = pointer.anchoredPosition;
        if (movingRight) pos.x += step;
        else pos.x -= step;

        float areaHalfWidth = playArea.rect.width / 2f;
        float pointerHalfWidth = pointer.rect.width / 2f;

        float minX = -areaHalfWidth + pointerHalfWidth;
        float maxX = areaHalfWidth - pointerHalfWidth;

        if (pos.x > maxX)
        {
            pos.x = maxX;
            movingRight = false;
        }
        else if (pos.x < minX)
        {
            pos.x = minX;
            movingRight = true;
        }

        pointer.anchoredPosition = pos;
    }

    void CheckResult()
    {
        // Use anchoredPosition.x to stay in the UI's local coordinate system
        float pointerX = pointer.anchoredPosition.x;
        float zoneX = perfectZone.anchoredPosition.x;
        
        // We need to know how far the "hit box" extends from the center of the zone
        float zoneHalfWidth = perfectZone.rect.width / 2f;

        // Check if the distance between the two is less than half the zone's width
        if (Mathf.Abs(pointerX - zoneX) <= zoneHalfWidth)
        {
            Debug.Log("PERFECT / GOOD HIT");
            Success();
        }
        else
        {
            Debug.Log("BAD HIT");
            Fail();
        }
    }

    void Success()
    {
        // The harder the game gets, the more "relief" a success gives
        float relief = 3f * SuspicionSystem.Instance.currentDifficultyMult;
        SuspicionSystem.Instance.ReduceSuspicion(relief);
        StopCheck();
        gameObject.SetActive(false);
        OnCheckFinished?.Invoke();
        if (AudioManager.instance != null)
        AudioManager.instance.PlaySound(AudioManager.instance.Good_skill);
    }

    void Fail()
    {
        Debug.Log("FAIL");

        SuspicionSystem.Instance.AddSuspicion(10f);
        StopCheck();
        gameObject.SetActive(false);

        OnCheckFinished?.Invoke();
        if (AudioManager.instance != null)
        AudioManager.instance.PlaySound(AudioManager.instance.Bad_skill);
    }

    public void StartCheck()
    {
        isActive = true;

        RandomizePerfectZone();

        timer = timeToStartPenalty;
        penaltyActive = false;

        Wind.Instance.OnSkillCheckStart();
    }

    public void StopCheck()
    {
        isActive = false;

        Wind.Instance.OnSkillCheckEnd();
    }
}