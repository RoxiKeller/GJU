using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsPanel;
    public bool isPaused = false;
    public Toggle muteToggle;
    public Slider masterSlider;
    public AudioMixer mainMixer;

    private void Start()
    {
        // Ne asigurăm că totul e închis la început
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        Time.timeScale = 1f; // Siguranță maximă
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Dacă e deschis panoul de setări, Escape ar trebui să închidă doar setările
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                CloseSettings();
            }
            else if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            pauseMenuUI.SetActive(false);
        }
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu");
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
}