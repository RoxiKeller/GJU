using UnityEngine;

[System.Serializable]
public class NPCStory
{
    [TextArea(2, 5)]
    public string storyText;
    public string requiredMask; // Must match "Happy" or "Sad" exactly
    public float suspicionPenalty = 10f;
}