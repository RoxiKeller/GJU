using UnityEngine;
using TMPro; // Ensure you have TextMeshPro installed
using System.Collections;

public class NPC : MonoBehaviour
{
    [Header("References")]
    public King king;
    public GameObject speechBubble; 
    public TextMeshProUGUI dialogueText;

    [Header("Movement Settings")]
    public Transform inspectionPoint; 
    public float walkSpeed = 2f;
    public float noticeThreshold = 6f; 

    [Header("Talking Settings")]
    public float displayDuration = 3f;

    private Vector3 startPosition;
    private bool isInspecting = false;
    private string currentReason = "";
    private Coroutine dialogueCoroutine;

    void Start()
    {
        startPosition = transform.position;
        if (speechBubble != null) speechBubble.SetActive(false);
    }

    void Update()
    {
        // 1. Logic to START moving toward the King
        if (!isInspecting)
        {
            if (king.blinkTimer > noticeThreshold)
            {
                StartInspection("He's not blinking...");
            }
        }

        // 2. Handle Movement
        Vector3 target = isInspecting ? inspectionPoint.position : startPosition;
        transform.position = Vector3.MoveTowards(transform.position, target, walkSpeed * Time.deltaTime);

        // 3. Logic to LEAVE
        if (isInspecting && currentReason == "His eyes are twitching!" && king.blinkTimer > 2f)
        {
            EndInspection("Must have been the wind.");
        }
    }

    public void StartInspection(string reason)
    {
        if (isInspecting) return;
        isInspecting = true;
        currentReason = reason;

        if (reason == "He's not blinking...") Say("Is the King... okay?");
        else Say("His eyes are twitching!");
    }

    public void OnKingBlink()
    {
        if (isInspecting && currentReason == "He's not blinking...")
        {
            EndInspection("Oh, he's fine.");
        }
    }

    void EndInspection(string partingWords)
    {
        isInspecting = false;
        currentReason = "";
        Say(partingWords);
    }

    // --- Talking Logic ---
    public void Say(string message)
    {
        if (dialogueCoroutine != null) StopCoroutine(dialogueCoroutine);
        dialogueCoroutine = StartCoroutine(ShowText(message));
    }

    private IEnumerator ShowText(string message)
    {
        speechBubble.SetActive(true);
        dialogueText.text = message;
        yield return new WaitForSeconds(displayDuration);
        speechBubble.SetActive(false);
    }
}