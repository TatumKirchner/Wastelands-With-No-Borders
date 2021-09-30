using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private IsPaused pauseManager;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private PlayerHealth playerHealth;
    private bool pauseOpen = false;

    private void Start()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

    private void Update()
    {
        //Get our input.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
    }

    //Open and closes the pause menu.
    void OpenPauseMenu()
    {
        pauseOpen = !pauseOpen;

        if (pauseOpen && !playerHealth.dead)
        {
            pauseManager.paused = true;
            pausePanel.SetActive(true);
        }
        else
        {
            pauseManager.paused = false;
            pausePanel.SetActive(false);
            optionsPanel.SetActive(false);
            controlsPanel.SetActive(false);
        }
    }

    //Resume the game.
    public void Play()
    {
        pausePanel.SetActive(false);
        pauseManager.paused = false;
        pauseOpen = false;
    }

    //Open the options panel.
    public void Options()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    //Restart the game.
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Quit the game
    public void Quit()
    {
        Application.Quit();
    }
    
    //Back out of the options menu.
    public void Back()
    {
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    //Set the game to be Windowed.
    public void WindowedScreenMode()
    {
        Screen.fullScreen = false;
    }

    //Set the game to be full screen.
    public void FullScreenMode()
    {
        Screen.fullScreen = true;
    }

    //Open the controls panel.
    public void Controls()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    //Back out of the controls panel.
    public void ControlsBack()
    {
        controlsPanel.SetActive(false);
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    //Load the main menu.
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
