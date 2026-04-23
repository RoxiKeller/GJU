using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KingFaceSlot : MonoBehaviour
{
    // This is the "EquippedMaskLayer" child object
    public SpriteRenderer maskLayerRenderer; 

    public string currentlyEquippedMask = "None"; // Start with no mask

    // Assign these in the Inspector
    public Sprite happyFaceSprite;
    public Sprite sadFaceSprite;

    void Start()
    {
        // Start with the mask layer invisible/empty
        maskLayerRenderer.sprite = null;
    }

    public void EquipMask(DraggableMask mask)
    {
        currentlyEquippedMask = mask.maskType;
        
        // Change the sprite on the top layer
        if (currentlyEquippedMask == "Happy")
            maskLayerRenderer.sprite = happyFaceSprite;
        else if (currentlyEquippedMask == "Sad")
            maskLayerRenderer.sprite = sadFaceSprite;

        // Add that crooked "cardboard" look
        maskLayerRenderer.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-5f, 5f));
        
        Debug.Log("Slapped a " + currentlyEquippedMask + " mask on the King!");
    }

    void OnMouseDown()
    {
        // If the player clicks the King's face while a mask is on...
        if (currentlyEquippedMask != "None")
        {
            // Remove the mask
            maskLayerRenderer.sprite = null;
            currentlyEquippedMask = "None";
            
            Debug.Log("The King's mask fell off!");
            // Play a "paper sliding" or "rip" sound
        }
    }
}