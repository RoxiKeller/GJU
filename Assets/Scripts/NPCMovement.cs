using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("Movement")]
    public Transform targetPoint;
    public float moveSpeed = 2f;
    public float stopDistance = 0.1f;

    private bool hasArrived = false;

    void Update()
    {
        if (hasArrived) return;

        MoveToTarget();
    }

    void MoveToTarget()
    {
        float distance = Vector2.Distance(transform.position, targetPoint.position);

        if (distance > stopDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPoint.position,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            hasArrived = true;
            OnArrival();
        }
    }

    void OnArrival()
    {
        Debug.Log("NPC arrived! Start interaction.");

        // Later:
        // Trigger dialogue
        // Start suspicion check
        // Guard greets king
    }
}