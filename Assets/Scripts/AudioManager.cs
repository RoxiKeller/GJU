using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton: permite accesul global

    public AudioSource sfxSource; // Sursa care va scoate sunetul

    public AudioClip Bark1;
    public AudioClip Bark2; // Clip pentru lătrat de câine
    public AudioClip Alarm;
    public AudioClip Bad_skill;
    public AudioClip Good_skill;
    public AudioClip CK_intro;
    public AudioClip CK_victory;
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
        // Ne asigurăm că există un singur AudioManager în tot jocul
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject); // Nu se distruge când schimbi scena
        } else {
            Destroy(gameObject);
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
}