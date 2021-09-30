using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private IsPaused paused;

    private void Update()
    {
        if (paused.paused)
        {
            source.Pause();
        }
        else
        {
            source.UnPause();
        }
    }
}
