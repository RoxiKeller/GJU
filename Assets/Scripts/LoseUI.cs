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
}