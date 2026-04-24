using UnityEngine;
using TMPro;
using System.Collections;

public class AnnouncementUI : MonoBehaviour
{
    public static AnnouncementUI Instance;

    public TextMeshProUGUI announcementText;
    public CanvasGroup canvasGroup; // Used for smooth fading
    public float fadeDuration = 1f;
    public float stayDuration = 2f;

    private Coroutine displayCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Hide it immediately at start
        if (canvasGroup != null) canvasGroup.alpha = 0;
    }

    public void Display(string message)
    {
        // If we are already showing this exact message, don't restart!
        if (displayCoroutine != null && announcementText.text == message && canvasGroup.alpha > 0) 
        {
            return; 
        }

        if (displayCoroutine != null) StopCoroutine(displayCoroutine);
        displayCoroutine = StartCoroutine(FadeSequence(message));
    }

    private IEnumerator FadeSequence(string message)
    {
        announcementText.text = message;

        // Fade In
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            yield return null;
        }

        // Wait
        yield return new WaitForSeconds(stayDuration);

        // Fade Out
        timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 0;
    }
}