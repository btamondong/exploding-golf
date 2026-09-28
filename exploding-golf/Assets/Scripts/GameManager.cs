using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject endScreenCanvas;

    void Start()
    {
        mainMenuCanvas.SetActive(true);
        endScreenCanvas.SetActive(false);

        Time.timeScale = 0f;   // freeze game at menu
    }

    public void StartGame()
    {
        mainMenuCanvas.SetActive(false);
        Time.timeScale = 1f;   // unfreeze game
    }
    
    public void EndLevel()
    {
        endScreenCanvas.SetActive(true);
        Time.timeScale = 0f;   // freeze game at end screen
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;   // unfreeze before reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        endScreenCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);

        Time.timeScale = 0f;   // freeze again at menu
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}