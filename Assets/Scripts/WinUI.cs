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

    public void ShowWinScreen()
    {
        if (winPanel != null) winPanel.SetActive(true);

        // Switch to Victory Music
        if (AudioManager.instance != null)
        {
            // Stop gameplay, Start victory
            AudioManager.instance.ToggleLoop(AudioManager.instance.CK_gameplay, false);
            AudioManager.instance.ToggleLoop(AudioManager.instance.CK_victory, true);
        }

        Time.timeScale = 0f; // Pause the world
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
        Time.timeScale = 1f;
        AudioManager.instance.ToggleLoop(AudioManager.instance.CK_victory, false); // Stop victory
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ShowCredits()
    {
        Time.timeScale = 1f;
        MainMenuController.shouldOpenCreditsOnLoad = true;

        if (AudioManager.instance != null)
        {
            // Stop the Victory music specifically before leaving
            AudioManager.instance.ToggleLoop(AudioManager.instance.CK_victory, false);
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}