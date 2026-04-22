using UnityEngine;
using System.Collections;

public class Meat : MonoBehaviour
{
    public Vector3 hiddenPosition;
    public Vector3 visiblePosition;
    public float moveSpeed = 5f;
    public float activeDuration = 5f; // How long the meat stays on screen

    private bool isActive;
    public Dog dog; // Reference to the dog to tell it when meat is gone

    void Start()
    {
        transform.position = hiddenPosition;
        isActive = false;
    }

    public void ShowMeat()
    {
        StopAllCoroutines();
        isActive = true;
        
        // Tells the dog to come running
        if (dog != null) dog.Distract();

        StartCoroutine(MeatSequence());
    }

    private IEnumerator MeatSequence()
    {
        // 1. Move to the screen
        yield return StartCoroutine(MoveTo(visiblePosition));

        // 2. Wait for the duration
        yield return new WaitForSeconds(activeDuration);

        // 3. Move back off-screen
        yield return StartCoroutine(MoveTo(hiddenPosition));

        // 4. Reset state and tell the dog it's gone
        isActive = false;
        if (dog != null) dog.ResetDistraction();
    }

    public void HideMeat() // Manual hide if needed
    {
        StopAllCoroutines();
        isActive = false;
        StartCoroutine(MoveTo(hiddenPosition));
    }

    IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                target,
                Time.deltaTime * moveSpeed
            );
            yield return null;
        }
        transform.position = target;
    }

    public bool IsActive() => isActive;
}