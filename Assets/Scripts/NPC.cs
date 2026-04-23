using UnityEngine;
using TMPro;
using System.Collections;

public class NPC : MonoBehaviour
{
    [Header("Base NPC Settings")]
    public King king;
    public GameObject speechBubble;
    public TextMeshProUGUI dialogueText;
    public float displayDuration = 3f;

    [Header("Movement")]
    public Transform inspectionPoint;
    public float walkSpeed = 2f;
    public float stoppingDistance = 5f;

    [Header("Suspicion Settings")]
    public float maxWaitTimeBeforeSuspicion = 3f;
    public float suspicionRate = 5f;
    protected float currentInspectionTimer;

    protected Vector3 startPosition;
    protected bool isInspecting;
    protected string currentReason;
    protected Coroutine dialogueCoroutine;

    [Header("Random Spawning Area")]
    public BoxCollider2D spawnArea;

    protected virtual void Start()
    {
        if (spawnArea != null)
        {
            transform.position = GetRandomPointInBounds(spawnArea.bounds);
        }

        startPosition = transform.position;
        if (speechBubble != null) speechBubble.SetActive(false);

        // Inside Start()
        walkSpeed = Random.Range(1.5f, 3.5f);
    }

    protected virtual void Update()
    {
        HandleMovement();
        HandleSuspicionTimer();
    }

    private Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            transform.position.z // Keep the NPC's original Z depth
        );
    }

    protected void HandleSuspicionTimer()
    {
        if (isInspecting && HasReachedTarget())
        {
            currentInspectionTimer += Time.deltaTime;

            if (currentInspectionTimer > maxWaitTimeBeforeSuspicion)
            {
                // Only bark/shout if we haven't already started this phase
                if (currentReason != "Angry") 
                {
                    OnSuspicionThresholdReached();
                }
                SuspicionSystem.Instance.AddSuspicion(suspicionRate * Time.deltaTime);
            }
        }
        else
        {
            currentInspectionTimer = 0f;
        }
    }

    // New helper to check if NPC is at their spot
    protected bool HasReachedTarget()
    {
        return Vector3.Distance(transform.position, inspectionPoint.position) <= stoppingDistance + 0.1f;
    }

    // Allow child classes to define their own "Angry" shout
    protected virtual void OnSuspicionThresholdReached()
    {
        currentReason = "Angry";
        Say("Something is wrong here...");
    }

    protected virtual void OnEnable()
    {
        king.OnBlinkEvent += OnKingBlink;
    }

    protected virtual void OnDisable()
    {
        king.OnBlinkEvent -= OnKingBlink;
    }

    protected void HandleMovement()
    {
        Vector3 target = isInspecting ? inspectionPoint.position : startPosition;
        
        // Calculate the distance to the target
        float distanceToTarget = Vector3.Distance(transform.position, target);

        // If we are inspecting, stop at stoppingDistance. 
        // If we are returning home, go all the way (stoppingDistance = 0 for home).
        float currentStoppingDistance = isInspecting ? stoppingDistance : 0.05f;

        if (distanceToTarget > currentStoppingDistance)
        {
            // Calculate a direction vector toward the target
            Vector3 direction = (target - transform.position).normalized;
            
            // Calculate where the NPC actually wants to stop
            Vector3 stoppingPoint = target - (direction * currentStoppingDistance);

            transform.position = Vector3.MoveTowards(
                transform.position,
                stoppingPoint,
                walkSpeed * Time.deltaTime
            );
        }
    }

    public virtual void StartInspection(string reason)
    {
        // If they are already inspecting the King for blinking, 
        // don't let the meat-throwing distract them from their suspicion.
        if (isInspecting && (currentReason.Contains("blink") || currentReason == "Angry")) 
            return;

        isInspecting = true;
        currentReason = reason;
        Say(reason);
    }

    public virtual void EndInspection(string message)
    {
        isInspecting = false;
        currentReason = "";
        currentInspectionTimer = 0f; // Reset the "standing still" timer!
        Say(message);
    }

    public virtual void OnKingBlink()
    {
        // overridden by child classes
    }

    public void Say(string message)
    {
        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);

        dialogueCoroutine = StartCoroutine(ShowText(message));
    }

    private IEnumerator ShowText(string message)
    {
        speechBubble.SetActive(true);
        dialogueText.text = message;

        yield return new WaitForSeconds(displayDuration);

        speechBubble.SetActive(false);
    }
}