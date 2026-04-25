using UnityEngine;

public class Villager : NPC
{
    [Header("Animations & Visuals")]
    public Animator villagerAnimator;
    public SpriteRenderer villagerSprite;

    [Header("Blink Detection Settings")]
    public float noticeThreshold = 6f; // Time without blinking
    public int blinkCountThreshold = 3; // Max blinks allowed in the window
    public float blinkWindow = 3f; // Time window to track blinks
    
    private int recentBlinks = 0;
    private float windowTimer = 0f;
    private Vector3 lastPosition;

    protected override void Start()
    {
        base.Start();
        lastPosition = transform.position; // Initialize lastPosition
    }

    protected override void Update()
    {
        base.Update(); // Handles walking and suspicion logic from NPC.cs

        HandleVisuals(); // Handle animation and flipping
        HandleBlinkDetection(); // Keep the logic clean
    }

    private void HandleVisuals()
    {
        // Calculate movement this frame
        float movementDelta = (transform.position - lastPosition).magnitude;
        bool isMoving = movementDelta > 0.001f;

        // Update Animator
        if (villagerAnimator != null)
        {
            villagerAnimator.SetBool("isWalking", isMoving);
        }

        // Handle Sprite Flipping
        if (isMoving)
        {
            float directionX = transform.position.x - lastPosition.x;
            if (Mathf.Abs(directionX) > 0.001f)
            {
                villagerSprite.flipX = directionX > 0;
            }
        }

        lastPosition = transform.position;
    }

    private void HandleBlinkDetection()
    {
        // 1. Logic for leaving if player stops blinking too much
        if (isInspecting && currentReason == "Why is he blinking so much?!")
        {
            if (king.blinkTimer > 4f) 
            {
                EndInspection("I must have imagined it.");
            }
        }

        // 2. Logic for starting inspection if player isn't blinking enough
        if (!isInspecting && king.blinkTimer > noticeThreshold)
        {
            StartInspection("He's not blinking...");
        }

        // 3. Reset the "Blinking Too Much" timer window
        if (recentBlinks > 0)
        {
            windowTimer += Time.deltaTime;
            if (windowTimer > blinkWindow)
            {
                recentBlinks = 0;
                windowTimer = 0f;
            }
        }
    }

    protected override void OnSuspicionThresholdReached()
    {
        base.OnSuspicionThresholdReached();
        Say("Why is he just staring at me?!");
    }

    public override void OnKingBlink()
    {
        if (isInspecting && (currentReason == "He's not blinking..." || currentReason == "Angry"))
        {
            string message = (currentReason == "Angry") ? "Hmph. At least he's alive." : "Oh, he's fine.";
            EndInspection(message);
            recentBlinks = 0; 
            return;
        }

        if (currentReason != "Why is he blinking so much?!")
        {
            recentBlinks++;
            if (!isInspecting && recentBlinks >= blinkCountThreshold)
            {
                StartInspection("Why is he blinking so much?!");
            }
        }
    }
}