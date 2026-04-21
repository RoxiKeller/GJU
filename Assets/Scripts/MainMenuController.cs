using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    public GameObject settingsPanel;
    public AudioMixer mainMixer;

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void SetMasterVolume(float volume)
    {
        mainMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
    }
    
    public void SetSFXVolume(float volume)
    {
        mainMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }
    
    public void SetMute(bool isMuted)
    {
        if (isMuted) mainMixer.SetFloat("MasterVol", -80f);
        else mainMixer.SetFloat("MasterVol", 0f);
    }
    
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

        EditorApplication.isPlaying = false;
    }
}