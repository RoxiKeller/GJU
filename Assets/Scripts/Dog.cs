using UnityEngine;
using System.Collections;

public class Dog : NPC
{
    public Meat meat;

    [Header("Random Inspection Settings")]
    public float minWaitTime = 5f;
    public float maxWaitTime = 15f;
    
    // Note: Use 'suspicionRate' and 'maxWaitTimeBeforeSuspicion' 
    // from the base class inspector to tune the dog's anger!

    private bool distracted;
    private float nextInspectionTimer;

    protected override void Start()
    {
        base.Start();
        DetermineNextInspectionTime();
    }

    protected override void Update()
    {
        if (distracted) { MoveToMeat(); return; }

        // This handles: HandleMovement, HandleSuspicionTimer, and currentInspectionTimer reset
        base.Update(); 

        if (!isInspecting)
        {
            nextInspectionTimer -= Time.deltaTime;
            if (nextInspectionTimer <= 0) 
            {
                StartInspection("Sniff sniff...");
                // currentInspectionTimer is reset to 0 automatically by base.Update logic
            }
        }
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
        nextInspectionTimer = Random.Range(minWaitTime, maxWaitTime);
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

    public bool IsInspectingKing()
    {
        // The dog is only "active" if it is inspecting AND has reached the king
        return isInspecting && HasReachedTarget();
    }

    public bool IsMovingToOrInspectingKing()
    {
        // If the dog's 'isInspecting' is true, it means it is either
        // walking toward the King or already there growling.
        return isInspecting && !distracted;
    }
}