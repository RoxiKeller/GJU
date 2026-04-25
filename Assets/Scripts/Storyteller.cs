using UnityEngine;

public class Storyteller : NPC
{
    [Header("Story Cycle Settings")]
    public float minTimeBetweenStories = 10f;
    public float maxTimeBetweenStories = 20f;
    private float nextArrivalTimer;

    [Header("Story Content")]
    public NPCStory[] storyPool;
    private NPCStory currentStory;

    [Header("Judging Settings")]
    public float timeToListen = 8f;     // Total time they stay
    public float gracePeriod = 3f;      // Seconds player has to react for free
    private float listeningTimer;
    private float currentGraceTimer;

    [Header("Visual Variety")]
    public StorytellerLook[] availableLooks;
    public Animator storytellerAnimator;
    public SpriteRenderer storytellerSprite;
    
    private Sprite currentIdleSprite; // To remember which idle to use
    private Vector3 lastPosition;

    protected override void Start()
    {
        base.Start();
        lastPosition = transform.position;
        ApplyRandomLook();
    }

    protected override void Update()
    {
        HandleMovement(); 
        HandleVisuals();
        
        if (!isInspecting)
        {
            nextArrivalTimer -= Time.deltaTime;
            if (nextArrivalTimer <= 0) TriggerNewStory();
        }
        else if (HasReachedTarget())
        {
            // 1. Tick down the total stay time
            listeningTimer -= Time.deltaTime;
            
            // 2. Tick down the grace period
            if (currentGraceTimer > 0)
            {
                currentGraceTimer -= Time.deltaTime;
            }
            else
            {
                // 3. Grace period is OVER. Now we judge.
                CheckMaskRequirement();
            }

            if (listeningTimer <= 0) FinishStory();
        }
    }

    public void ApplyRandomLook()
    {
        if (availableLooks != null && availableLooks.Length > 0)
        {
            StorytellerLook chosenLook = availableLooks[Random.Range(0, availableLooks.Length)];
            
            // Store the idle sprite for this specific look
            currentIdleSprite = chosenLook.idleSprite;

            if (storytellerAnimator != null)
            {
                storytellerAnimator.runtimeAnimatorController = chosenLook.animatorController;
            }
        }
    }

    void TriggerNewStory()
    {
        if (storyPool != null && storyPool.Length > 0)
        {
            // Pick the look first!
            ApplyRandomLook(); 

            currentStory = storyPool[Random.Range(0, storyPool.Length)];
            listeningTimer = timeToListen; 

            float mult = SuspicionSystem.Instance.currentDifficultyMult;
            currentGraceTimer = gracePeriod / mult; 
            
            base.StartInspection(currentStory.storyText);
        }
    }

    void CheckMaskRequirement()
    {
        KingFaceSlot faceSlot = Object.FindAnyObjectByType<KingFaceSlot>();

        if (faceSlot != null && currentStory != null)
        {
            // Only add suspicion if the mask is wrong AFTER grace period
            if (faceSlot.currentlyEquippedMask != currentStory.requiredMask)
            {
                SuspicionSystem.Instance.AddSuspicion(suspicionRate * Time.deltaTime);
            }
        }
    }

    void FinishStory()
    {
        KingFaceSlot faceSlot = Object.FindAnyObjectByType<KingFaceSlot>();
        string farewell = "Thank you for listening.";

        // Final check: If they leave and it's still wrong, big penalty!
        if (faceSlot != null && faceSlot.currentlyEquippedMask != currentStory.requiredMask)
        {
            farewell = "You don't seem to care at all!";
            SuspicionSystem.Instance.AddSuspicion(15f); 
        }

        if (faceSlot != null && faceSlot.currentlyEquippedMask == currentStory.requiredMask)
        {
            farewell = "Ah, your Majesty truly understands me!";
            SuspicionSystem.Instance.ReduceSuspicion(10f); // Big reward for a job well done!
        }

        EndInspection(farewell);
        SetRandomArrivalTimer();
    }

    void SetRandomArrivalTimer()
    {
        // If mult is 1.0, wait is normal. If mult is 2.0, wait is halved!
        float mult = SuspicionSystem.Instance.currentDifficultyMult;
        nextArrivalTimer = Random.Range(minTimeBetweenStories, maxTimeBetweenStories) / mult;
    }

    private void HandleVisuals()
    {
        float movementDelta = (transform.position - lastPosition).magnitude;
        bool isMoving = movementDelta > 0.001f;

        if (storytellerAnimator != null)
        {
            // Disable animator if not moving so it doesn't fight our manual sprite swap
            storytellerAnimator.enabled = isMoving; 
            
            if (isMoving)
            {
                storytellerAnimator.SetBool("isWalking", true);
            }
            else
            {
                // Force the idle sprite when stopped
                storytellerSprite.sprite = currentIdleSprite;
            }
        }

        if (isMoving)
        {
            float directionX = transform.position.x - lastPosition.x;
            if (Mathf.Abs(directionX) > 0.01f)
            {
                // Flip logic: assuming sprite faces right by default
                storytellerSprite.flipX = directionX > 0;
            }
        }
        lastPosition = transform.position;
    }

    public override void EndInspection(string message)
    {
        base.EndInspection(message);
        // Optional: Change look for the NEXT time they arrive
    }
}