using UnityEngine;

public class IsPaused : MonoBehaviour
{
    public bool paused = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        PauseGame();
    }

    //When the paused flag is true set time scale to 0 and pause the audio.
    public void PauseGame()
    {
        if (paused)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
        }
    }
}
