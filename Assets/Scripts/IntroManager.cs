using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI introText;
    public CanvasGroup fadeGroup;
    public RectTransform backgroundTransform;

    [Header("Scene Settings")]
    public string nextSceneName = "GameScene";

    [Header("Timing")]
    public float textDelay = 4f; // Time per line
    public float fadeSpeed = 1.5f;
    
    [Header("Scrolling")]
    public float startX = 0f;
    public float endX = -5625f; // Adjust this until the end of the image is reached
    public float speed = 0.5f;      // Cât de repede pulsează
    public float amplitude = 0.1f; // Cât de mult se mărește (0.1 = 10%)
    Vector3 initialScale;
    public GameObject space_text;

    private string[] lines = {
        "During the glorious year of 506... a golden age!",
        "Bards were celebrated across the land—",
        "(cough) especially one in particular. Me.",
        "Feasts were endless...",
        "The wine flowed...",
        "The dances were exquisite...",
        "(and oh... the women...)",
        "The valleys overflowed with milk and honey—",
        "and absolutely no one had a single problem.",
        "We also had... a king.",
        "Carefree.",
        "Ignorant.",
        "Magnificently useless.",
        "...So naturally—he dies.",
        "Not in battle. Not heroically.",
        "...He choked.",
        "...On a walnut.",
        "Which is particularly unfortunate...",
        "...because His Majesty was violently allergic to walnuts.",
        "...And who served the walnuts?",
        "...Me.",
        "Now—normally this would be a minor inconvenience...",
        "...IF he had an heir.",
        "He did not.",
        "...IF his next in line wasn’t a cruel, spoiled disaster of a nephew.",
        "He is.",
        "So now... the king is dead.",
        "...No one must find out.",
        "For the good of the kingdom.",
        "...and more importantly... for my sake."
    };

    void Start()
    {
        initialScale = transform.localScale;
        StartCoroutine(PlayIntro());
        AudioManager.instance.PlaySound(AudioManager.instance.CK_intro);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {Invoke("LoadNextScene",1f);
        StartCoroutine(FadeOut());
        }
        float pulse = 1f + Mathf.Sin(Time.time * speed) * amplitude;
        space_text.transform.localScale = initialScale * pulse;
    }

    IEnumerator PlayIntro()
    {
        if (fadeGroup != null) yield return StartCoroutine(FadeIn());

        // Calculate total duration so the scroll matches the text
        float totalDuration = lines.Length * textDelay;
        float elapsed = 0f;

        for (int i = 0; i < lines.Length; i++)
        {
            introText.text = lines[i];
            
            // Sub-timer for this specific line
            float lineTimer = 0f;
            while (lineTimer < textDelay)
            {
                lineTimer += Time.deltaTime;
                elapsed += Time.deltaTime;

                // Move background based on overall progress (0 to 1)
                float t = elapsed / totalDuration;
                float currentX = Mathf.Lerp(startX, endX, t);
                
                backgroundTransform.anchoredPosition = new Vector2(currentX, backgroundTransform.anchoredPosition.y);
                
                yield return null;
            }
        }

        yield return new WaitForSeconds(1f);
        if (fadeGroup != null) yield return StartCoroutine(FadeOut());
        LoadNextScene();
    }

    IEnumerator FadeIn()
    {
        fadeGroup.alpha = 1f;

        while (fadeGroup.alpha > 0f)
        {
            fadeGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        fadeGroup.alpha = 0f;
    }

    IEnumerator FadeOut()
    {
        fadeGroup.alpha = 0f;

        while (fadeGroup.alpha < 1f)
        {
            fadeGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        fadeGroup.alpha = 1f;
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}