using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene detection

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource sfxSource;
    public AudioSource musicSource;

    public AudioClip Blink;
    public AudioClip Bark1;
    public AudioClip Bark2; // Clip pentru lătrat de câine
    public AudioClip Alarm;
    public AudioClip Bad_skill;
    public AudioClip Good_skill;
    public AudioClip CK_intro;
    public AudioClip CK_gameplay;
    public AudioClip CK_victory;
    public AudioClip CK_failure;
    public AudioClip Dog_eat;
    public AudioClip Dog_grrr;
    public AudioClip Hmm1;
    public AudioClip Hmm2;
    public AudioClip Hmm3;
    public AudioClip Oh_no;
    public AudioClip Mask_on;
    public AudioClip Mask_off;
    public AudioClip Wind;
    // 1. Add the variable at the top if you haven't
    public AudioClip CK_menu;


    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return; // Stop execution here
        }
    }

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This runs every time a new scene is loaded
    // 2. Update the Scene Detection
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            ToggleLoop(CK_gameplay, true);
        }
        else if (scene.name == "Intro")
        {
            ToggleLoop(CK_intro, true);
        }
        else if (scene.name == "MainMenu") // Specifically check for the Menu
        {
            ToggleLoop(CK_menu, true);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null) {
            sfxSource.PlayOneShot(clip);
        } else {
            Debug.LogWarning("Ai uitat să bagi clipul audio, patroane!");
        }
    }
    public void ToggleLoop(AudioClip clip, bool shouldLoop)
    {
        if (clip == null) return;

        if (shouldLoop)
        {
            // If this song is ALREADY playing, don't restart it!
            if (musicSource.clip == clip && musicSource.isPlaying) return;

            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            musicSource.Stop();
            musicSource.clip = null; // Clear it out
            musicSource.loop = false;
        }
    }
}