using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject endScreenCanvas;

    void Start()
    {
        // Show main menu at start
        mainMenuCanvas.SetActive(true);
        endScreenCanvas.SetActive(false);

        // Freeze gameplay while menu is open
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        // Hide menu and unfreeze gameplay
        mainMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
    
    public void EndLevel()
    {
        // Show end screen and freeze gameplay
        endScreenCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        // Unfreeze before reloading so the new scene starts normally
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        // Hide end screen, show menu, freeze gameplay
        endScreenCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}