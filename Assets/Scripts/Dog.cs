using UnityEngine;
using System.Collections;

public class Dog : NPC
{
    public Meat meat;

    [Header("Animations")]
    public Animator dogAnimator;
    public SpriteRenderer dogSprite;

    [Header("Random Inspection Settings")]
    public float minWaitTime = 5f;
    public float maxWaitTime = 15f;

    private bool distracted;
    private float nextInspectionTimer;
    private Vector3 lastPosition;

    protected override void Start()
    {
        base.Start();
        lastPosition = transform.position;
        DetermineNextInspectionTime();
    }

    protected override void Update()
    {
        // 1. Handle actual logic
        if (distracted) { MoveToMeat(); }
        else { base.Update(); }

        // 2. Handle Visuals (Animations and Flipping)
        HandleVisuals();

        // 3. Spawning/Inspection logic
        if (!isInspecting && !distracted)
        {
            nextInspectionTimer -= Time.deltaTime;
            if (nextInspectionTimer <= 0) 
            {
                StartInspection("Sniff sniff...");
            }
        }
    }

    private void HandleVisuals()
    {
        // Calculate velocity based on movement since last frame
        float movementDelta = (transform.position - lastPosition).magnitude;
        bool isMoving = movementDelta > 0.001f; // Threshold to avoid micro-jitters

        // Update Animator (Assumes you have a bool parameter named 'isWalking')
        if (dogAnimator != null)
        {
            dogAnimator.SetBool("isWalking", isMoving);
        }

        // Handle Flipping
        if (isMoving)
        {
            float directionX = transform.position.x - lastPosition.x;
            
            // If moving right (pos X), flip false. If moving left (neg X), flip true.
            // Note: This depends on your sprite's default facing direction.
            if (Mathf.Abs(directionX) > 0.001f)
            {
                dogSprite.flipX = directionX > 0;
            }
        }

        lastPosition = transform.position;
    }

    protected override void OnSuspicionThresholdReached()
    {
        base.OnSuspicionThresholdReached(); // Sets currentReason = "Angry"
        Say("GRRRR! BARK!");
    }

    public void Distract()
    {
        if (distracted) return; 

        distracted = true;
        isInspecting = false;
        // currentInspectionTimer resets to 0 automatically in next base.Update() call
        
        Say("Bork! MEAT!");
        DetermineNextInspectionTime();
    }

    public void ResetDistraction()
    {
        distracted = false;
    }

    private void DetermineNextInspectionTime()
    {
        float mult = SuspicionSystem.Instance.currentDifficultyMult;
        // As the game gets harder, the dog comes back significantly faster
        nextInspectionTimer = Random.Range(minWaitTime, maxWaitTime) / mult;
    }

    void MoveToMeat()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            meat.transform.position,
            walkSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, meat.transform.position) < 0.2f)
        {
            distracted = false;
        }
    }

    public bool IsMovingToOrInspectingKing()
    {
        // If the dog's 'isInspecting' is true, it means it is either
        // walking toward the King or already there growling.
        return isInspecting && !distracted;
    }
}