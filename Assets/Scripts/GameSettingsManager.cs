using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameSettingsManager : MonoBehaviour
{
    [Header("Giao diện UI")]
    public GameObject settingsPanel;

    [Header("Tên Scene Main Menu để quay về")]
    public string mainMenuSceneName = "MainMenu"; 

    private bool isPaused = false;

    void Start()
    {
        ResumeGame();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; 
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; 
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void ResetProgress()
    {
        Time.timeScale = 1f; 

        CemeteryHouseInteract.currentLevel = 0;
        PlayerPrefs.SetInt("SavedLevel", 0);


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}