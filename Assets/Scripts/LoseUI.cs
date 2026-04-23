using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseUI : MonoBehaviour
{
    // The Singleton instance
    public static LoseUI Instance;

    public GameObject losePanel;

    void Awake()
    {
        // Set up the Singleton
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        // Hide the panel at the start
        if (losePanel != null) losePanel.SetActive(false);
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}