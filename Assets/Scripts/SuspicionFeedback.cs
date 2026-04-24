using UnityEngine;
using TMPro;
using UnityEngine.UI; // Required for Image
using System.Collections;

public class SuspicionFeedback : MonoBehaviour
{
    public static SuspicionFeedback Instance;

    [Header("References")]
    public TextMeshProUGUI feedbackText;
    public CanvasGroup canvasGroup; // Assign the FeedbackContainer here
    public Image backgroundImage;   // Assign the Background Image here

    [Header("Settings")]
    public float timeToReset = 0.5f;
    public float displayDuration = 1.0f;
    public float fadeSpeed = 3f;

    private float currentChangeAmount = 0f;
    private float resetTimer = 0f;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        Instance = this;
        canvasGroup.alpha = 0f; // Start hidden
        feedbackText.text = "";
    }

    public void ShowChange(float amount)
    {
        currentChangeAmount += amount;
        resetTimer = timeToReset;

        // Update Text and Background Color
        if (currentChangeAmount > 0)
        {
            feedbackText.text = "+" + Mathf.RoundToInt(currentChangeAmount).ToString();
            feedbackText.color = Color.white;
            backgroundImage.color = new Color(0.8f, 0.2f, 0.2f, 0.8f); // Transparent Red
        }
        else if (currentChangeAmount < 0)
        {
            feedbackText.text = Mathf.RoundToInt(currentChangeAmount).ToString();
            feedbackText.color = Color.white;
            backgroundImage.color = new Color(0.2f, 0.8f, 0.2f, 0.8f); // Transparent Green
        }

        // Restart fade
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSequence());
    }

    void Update()
    {
        if (resetTimer > 0)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0) currentChangeAmount = 0;
        }
    }

    IEnumerator FadeSequence()
    {
        // Fade In
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(displayDuration);

        // Fade Out
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        
        feedbackText.text = "";
        currentChangeAmount = 0;
    }
}