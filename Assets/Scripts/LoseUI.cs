using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseUI : MonoBehaviour
{
    // The Singleton instance
    public static LoseUI Instance;

    public GameObject losePanel;

    void Awake()
    {
        // Simply set the instance to this one every time the scene loads
        Instance = this;

        if (losePanel != null) losePanel.SetActive(false);
        
        // Ensure time is moving! 
        // Sometimes another script might have set it to 0 before the scene reloaded.
        Time.timeScale = 1f; 
    }

    public void ShowLoseScreen()
    {
        // We turn on the panel, and if the script's object is off, turn it on too
        if (losePanel != null) losePanel.SetActive(true);
        
        gameObject.SetActive(true);
        Time.timeScale = 0f; // Freeze game
    }

    public void RestartGame()
    {
        // 1. Unfreeze first
        Time.timeScale = 1f;
        
        // 2. Clear any static events (Crucial!)
        // If your King.cs uses "OnBlinkEvent", you must clear those listeners
        // or they will try to talk to objects that were destroyed in the old scene.
        
        // 3. Reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}