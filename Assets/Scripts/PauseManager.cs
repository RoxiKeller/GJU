using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsPanel; // Trage panoul aici în Inspector
    public bool isPaused = false;

    public void Start()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }
    void Update()
    {
        // Verificăm dacă jucătorul apasă Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
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
        pauseMenuUI.SetActive(false); // Ascundem meniul
        Time.timeScale = 1f;          // Reluăm timpul
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);  // Arătăm meniul
        Time.timeScale = 0f;          // Înghețăm timpul
        isPaused = true;
    }

    public void OpenSettings()
    {
        Debug.Log("Deschidem setările... (Aici pui logica ta)");
        // Poți activa un alt panou de setări aici
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // FOARTE IMPORTANT: Resetăm timpul înainte de load!
        SceneManager.LoadScene("MainMenu"); // Pune numele exact al scenei tale
    }
}