using UnityEngine;

[CreateAssetMenu(fileName = "NewStorytellerLook", menuName = "Game/Storyteller Look")]
public class StorytellerLook : ScriptableObject
{
    public RuntimeAnimatorController animatorController; // The specific animator for this look
    public Color speechBubbleTint = Color.white;        // Optional: customized bubble color
    public Sprite idleSprite; // <--- ADD THIS
}