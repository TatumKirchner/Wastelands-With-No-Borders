using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer engineMixer;
    [SerializeField] private AudioMixer sfxMixer;

    private void Update()
    {
        //Get our input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionsPanel.SetActive(false);
        }
    }

    //Load the main scene.
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    //Open the options panel.
    public void OpenOptions()
    {
        startPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    //Quits the game.
    public void ExitGame()
    {
        Application.Quit();
    }

    //Sets the volume of the mixer to the slider value.
    //Using the Log10 to make the mixer volume reach 0 when the slider is all the way to the left.

    public void SetMusicVolume(float sliderValue)
    {
        musicMixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetEngineVolume(float sliderValue)
    {
        engineMixer.SetFloat("EngineVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSfxVolume(float sliderValue)
    {
        sfxMixer.SetFloat("sfxVol", Mathf.Log10(sliderValue) * 20);
    }

    //Set the game to windowed mode.
    public void WindowedScreenMode()
    {
        Screen.fullScreen = false;
    }

    //Sets the game to full screen.
    public void FullScreenMode()
    {
        Screen.fullScreen = true;
    }

    //Back out of the options panel.
    public void Back()
    {
        optionsPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    //Opens the controls panel.
    public void OpenControls()
    {
        startPanel.SetActive(false);
        optionsPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    //Backs out of the controls panel.
    public void ControlsBack()
    {
        controlsPanel.SetActive(false);
        optionsPanel.SetActive(false);
        startPanel.SetActive(true);
    }
}
