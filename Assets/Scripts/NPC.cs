using UnityEngine;
using TMPro;
using System.Collections;

public class NPC : MonoBehaviour
{
    [Header("References")]
    public King king;
    public GameObject speechBubble;
    public TextMeshProUGUI dialogueText;

    [Header("Movement")]
    public Transform inspectionPoint;
    public float walkSpeed = 2f;

    [Header("Talking")]
    public float displayDuration = 3f;

    protected Vector3 startPosition;
    protected bool isInspecting;
    protected string currentReason;
    protected Coroutine dialogueCoroutine;

    protected virtual void Start()
    {
        startPosition = transform.position;
        if (speechBubble != null)
            speechBubble.SetActive(false);
    }

    protected virtual void Update()
    {
        HandleMovement();
    }

    protected virtual void OnEnable()
    {
        king.OnBlinkEvent += OnKingBlink;
    }

    protected virtual void OnDisable()
    {
        king.OnBlinkEvent -= OnKingBlink;
    }

    protected void HandleMovement()
    {
        Vector3 target = isInspecting ? inspectionPoint.position : startPosition;
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            walkSpeed * Time.deltaTime
        );
    }

    public virtual void StartInspection(string reason)
    {
        if (isInspecting) return;

        isInspecting = true;
        currentReason = reason;

        Say(reason);
    }

    public virtual void EndInspection(string message)
    {
        isInspecting = false;
        currentReason = "";
        Say(message);
    }

    public virtual void OnKingBlink()
    {
        // overridden by child classes
    }

    public void Say(string message)
    {
        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);

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