using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    public static WinUI Instance;
    public GameObject winPanel;

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu"; // Make sure this matches your scene name exactly!

    void Awake()
    {
        Instance = this;
        if (winPanel != null) winPanel.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void ShowLoseScreen()
    {
        if (winPanel != null) winPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        AudioManager.instance.ToggleLoop(AudioManager.instance.CK_gameplay, false);
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

    public void ShowCredits()
    {
        // 1. Unfreeze time
        Time.timeScale = 1f;

        // 2. Set the "secret flag" in the Menu Controller
        MainMenuController.shouldOpenCreditsOnLoad = true;

        // 3. Clean up audio and load
        AudioManager.instance.ToggleLoop(AudioManager.instance.CK_gameplay, false);
        SceneManager.LoadScene(mainMenuSceneName);
    }
}