using UnityEngine;

public class Villager : NPC
{
    public float noticeThreshold = 6f;

    protected override void Update()
    {
        base.Update();

        if (!isInspecting && king.blinkTimer > noticeThreshold)
        {
            StartInspection("He's not blinking...");
        }
    }

    public override void OnKingBlink()
    {
        if (isInspecting && currentReason == "He's not blinking...")
        {
            EndInspection("Oh, he's fine.");
        }
    }
}