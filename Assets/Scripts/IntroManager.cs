using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI introText;
    public CanvasGroup fadeGroup; // optional for fade in/out

    [Header("Scene Settings")]
    public string nextSceneName = "GameScene";

    [Header("Timing")]
    public float textDelay = 3f;
    public float fadeSpeed = 2.5f;

    private string[] lines =
    {
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
        StartCoroutine(PlayIntro());
    }

    void Update()
    {
        // Press Space to skip intro
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextScene();
        }
    }

    IEnumerator PlayIntro()
    {
        if (fadeGroup != null)
            yield return StartCoroutine(FadeIn());

        for (int i = 0; i < lines.Length; i++)
        {
            introText.text = lines[i];
            yield return new WaitForSeconds(textDelay);
        }

        yield return new WaitForSeconds(1f);

        if (fadeGroup != null)
            yield return StartCoroutine(FadeOut());

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