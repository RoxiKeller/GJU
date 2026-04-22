using UnityEngine;

public class Villager : NPC
{
    public float noticeThreshold = 6f; // Time without blinking
    public int blinkCountThreshold = 3; // Max blinks allowed in the window
    public float blinkWindow = 3f; // Time window to track blinks
    
    private int recentBlinks = 0;
    private float windowTimer = 0f;

    protected override void Update()
    {
        base.Update(); // Handles walking and suspicion for standing too long

        // If he's inspecting because of too many blinks, 
        // and the player HASN'T blinked for a while, he leaves.
        if (isInspecting && currentReason == "Why is he blinking so much?!")
        {
            // If the King's eyes stay still for 4 seconds, the Villager loses interest
            if (king.blinkTimer > 4f) 
            {
                EndInspection("I must have imagined it.");
            }
        }

        // Handle the "Not Blinking" logic
        if (!isInspecting && king.blinkTimer > noticeThreshold)
        {
            StartInspection("He's not blinking...");
        }

        // Handle the "Blinking Too Much" logic window
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
        // 1. Resolve "Not Blinking" or "Angry" states immediately
        if (isInspecting && (currentReason == "He's not blinking..." || currentReason == "Angry"))
        {
            string message = (currentReason == "Angry") ? "Hmph. At least he's alive." : "Oh, he's fine.";
            EndInspection(message);
            recentBlinks = 0; 
            return;
        }

        // 2. Track "Too Much Blinking" frequency
        // We only count blinks if the NPC isn't already yelling at us for blinking!
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