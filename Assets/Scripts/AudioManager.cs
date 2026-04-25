using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton: permite accesul global

    public AudioSource sfxSource;
    public AudioSource musicSource; // Sursa care va scoate sunetul

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


    void Awake()
    {
        
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
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            musicSource.Stop();
            musicSource.loop = false;
        }
    }
}