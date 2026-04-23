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

    protected override void Start()
    {
        base.Start();
        SetRandomArrivalTimer();
    }

    protected override void Update()
    {
        HandleMovement(); 

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

    void TriggerNewStory()
    {
        if (storyPool != null && storyPool.Length > 0)
        {
            currentStory = storyPool[Random.Range(0, storyPool.Length)];
            listeningTimer = timeToListen; 
            currentGraceTimer = gracePeriod; // Start the free-time clock!
            
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

        EndInspection(farewell);
        SetRandomArrivalTimer();
    }

    void SetRandomArrivalTimer()
    {
        nextArrivalTimer = Random.Range(minTimeBetweenStories, maxTimeBetweenStories);
    }
}