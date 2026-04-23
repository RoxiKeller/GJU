using UnityEngine;
using UnityEngine.SceneManagement; // Required to reload the game
using UnityEngine.UI; // Required if you want to reference the Button via code

public class LoseUI : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject losePanel; // Drag your Lose Panel (the parent object) here

    void Awake()
    {
        // Ensure the lose screen is hidden when the game starts
        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }
    }

    // Call this method from your SuspicionSystem when the bar hits 100%
    public void ShowLoseScreen()
    {
        if (losePanel != null)
        {
            losePanel.SetActive(true);
            
            // Optional: Pause the game time so everything stops moving
            Time.timeScale = 0f; 
        }
    }

    // Link this to your Restart Button's "OnClick" event in the Inspector
    public void RestartGame()
    {
        // Resume time before restarting!
        Time.timeScale = 1f; 

        // Reloads the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}