using UnityEngine;

public class WindSkillCheck : MonoBehaviour
{
    public System.Action OnCheckFinished;

    [Header("Random Zone")]
    public float zonePadding = 50f;

    [Header("UI Elements")]
    public RectTransform pointer;
    public RectTransform bar;

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
        float barWidth = bar.rect.width;

        float minX = -barWidth / 2f + zonePadding;
        float maxX = barWidth / 2f - zonePadding;

        float randomX = Random.Range(minX, maxX);

        Vector2 newPos = perfectZone.anchoredPosition;
        newPos.x = randomX;

        perfectZone.anchoredPosition = newPos;
    }

    void MovePointer()
    {
        float step = speed * Time.deltaTime;

        Vector3 pos = pointer.localPosition;

        if (movingRight)
            pos.x += step * 200f;
        else
            pos.x -= step * 200f;

        float halfWidth = bar.rect.width / 2f;

        if (pos.x > halfWidth)
        {
            pos.x = halfWidth;
            movingRight = false;
        }
        else if (pos.x < -halfWidth)
        {
            pos.x = -halfWidth;
            movingRight = true;
        }

        pointer.localPosition = pos;
    }

    void CheckResult()
    {
        float pointerX = pointer.position.x;
        float zoneX = perfectZone.position.x;
        float zoneWidth = perfectZone.rect.width / 2f;

        if (Mathf.Abs(pointerX - zoneX) <= zoneWidth)
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