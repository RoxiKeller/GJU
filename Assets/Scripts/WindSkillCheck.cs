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
        float zoneHalfWidth = perfectZone.rect.width / 2f;

        float minX = -areaHalfWidth + zoneHalfWidth;
        float maxX = areaHalfWidth - zoneHalfWidth;

        float randomX = Random.Range(minX, maxX);

        Vector2 pos = perfectZone.anchoredPosition;
        pos.x = randomX;

        perfectZone.anchoredPosition = pos;
    }

    void MovePointer()
    {
        float step = speed * Time.deltaTime * 200f;

        Vector2 pos = pointer.anchoredPosition;

        if (movingRight)
            pos.x += step;
        else
            pos.x -= step;

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
        Debug.Log("SUCCESS");

        SuspicionSystem.Instance.ReduceSuspicion(2f);

        StopCheck();
        gameObject.SetActive(false);

        OnCheckFinished?.Invoke();
    }

    void Fail()
    {
        Debug.Log("FAIL");

        SuspicionSystem.Instance.AddSuspicion(10f);

        StopCheck();
        gameObject.SetActive(false);

        OnCheckFinished?.Invoke();
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