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

    public static bool shouldOpenCreditsOnLoad = false;
    
    void Start()
    {
        settingsPanel.SetActive(false);
        // Check if we came here from the Win Screen
        if (shouldOpenCreditsOnLoad)
        {
            OpenCredits();
            shouldOpenCreditsOnLoad = false; // Reset it so it doesn't happen every time!
        }
    }

    void Update() {
        if (creditsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape)) {
            CloseCredits();
        }
    }

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
            // Since we are in the Menu scene, Time.timeScale is 1, 
            // so standard "Normal" update mode works here!
            anim.Play("Credits_Crawl", 0, 0f);
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