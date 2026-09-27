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
    }

    public void StartGame()
    {
        mainMenuCanvas.SetActive(false);
    }
    
    public void EndLevel()
    {
        endScreenCanvas.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        endScreenCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}