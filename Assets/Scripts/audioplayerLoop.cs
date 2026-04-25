using UnityEngine;

public class audioplayerLoop : MonoBehaviour
{
    public AudioSource sfxSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlaySound(AudioClip clip)
    {
        if (clip != null) {
            sfxSource.PlayOneShot(clip);
        } else {
            Debug.LogWarning("Ai uitat să bagi clipul audio, patroane!");
        }
    }
}