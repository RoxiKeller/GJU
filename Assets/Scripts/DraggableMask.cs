using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DraggableMask : MonoBehaviour
{
    public string maskType; // "Happy" or "Sad"
    
    private Vector3 startPosition;
    private bool isDragging = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Camera mainCamera;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        startPosition = transform.position;
        mainCamera = Camera.main; // Cache the main camera for performance
    }

    // Unity automatically calls these functions on objects with colliders
    void OnMouseDown()
    {
        isDragging = true;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f); // Fade while dragging
        // Bring to front while dragging
        spriteRenderer.sortingOrder = 10; 
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        transform.position = mousePosition;

        // Use OverlapPointAll to find EVERYTHING under the mouse
        Vector2 mousePos2D = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos2D);
        
        bool overFace = false;
        foreach (var hit in hits)
        {
            if (hit.GetComponent<KingFaceSlot>())
            {
                overFace = true;
                break;
            }
        }

        if (overFace)
        {
            spriteRenderer.color = Color.green; 
        }
        else
        {
            // Use your original faded color
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        spriteRenderer.color = originalColor; // Restore color
        spriteRenderer.sortingOrder = 5; // Return to normal depth

        // Check if we are over the King's face slot
        CheckForDrop();
    }

    void CheckForDrop()
    {
        Vector2 mousePos2D = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos2D);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out KingFaceSlot slot))
            {
                slot.EquipMask(this);
                transform.position = startPosition;
                return; 
            }
        }

        // If we reach here, we didn't hit the face
        transform.position = startPosition;
    }
}