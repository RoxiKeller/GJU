using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    public AudioMixer mainMixer;

    [Header("Audio UI Refereneces")] 
    public Toggle muteToggle;
    public Slider masterSlider;
    

    public void PlayGame()
    {
        SceneManager.LoadScene("Intro");
    }
    
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void SetMasterVolume(float volume)
    {

        if (muteToggle != null && muteToggle.isOn)
        {
            mainMixer.SetFloat("MasterVol", -80f);
            return;
        }
        
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
        else SetMasterVolume(masterSlider.value);
    }
    
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);

        var anim = creditsPanel.GetComponentInChildren<Animator>();

        if (anim != null)
        {
            anim.Play("Credits_Crawl", 0, 0f);
            anim.Update(0);
        }
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}