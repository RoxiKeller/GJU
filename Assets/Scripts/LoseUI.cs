using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseUI : MonoBehaviour
{
    public static LoseUI Instance;
    public GameObject losePanel;

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu"; // Make sure this matches your scene name exactly!

    void Awake()
    {
        Instance = this;
        if (losePanel != null) losePanel.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void ShowLoseScreen()
    {
        if (losePanel != null) losePanel.SetActive(true);
        
        // Switch the music!
        if (AudioManager.instance != null)
        {
            // false on gameplay to stop it, then true on failure to start it
            AudioManager.instance.ToggleLoop(AudioManager.instance.CK_gameplay, false);
            AudioManager.instance.ToggleLoop(AudioManager.instance.CK_failure, true);
        }

        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        // Stop BOTH just in case
        AudioManager.instance.ToggleLoop(AudioManager.instance.CK_gameplay, false);
        AudioManager.instance.ToggleLoop(AudioManager.instance.CK_failure, false); 
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // --- NEW MAIN MENU LOGIC ---
    public void GoToMainMenu()
    {
        // 1. IMPORTANT: Unfreeze the engine
        Time.timeScale = 1f;

        // 2. Load the menu scene
        AudioManager.instance.ToggleLoop(AudioManager.instance.CK_gameplay, false);
        SceneManager.LoadScene(mainMenuSceneName);
        
    }
}