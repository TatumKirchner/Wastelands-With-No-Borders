using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer masterMixer;
    public AudioMixer engineMixer;
    public AudioMixer sfxMixer;

    //Sets the mixer volume to what the slider value is.
    //Used Mathf.Log10 so if the slider is all the way to 0 the volume will not be heard in game.
    public void SetMasterLevel(float sliderValue)
    {
        masterMixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetEngineLevel(float sliderValue)
    {
        engineMixer.SetFloat("EngineVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSfxLevel(float sliderValue)
    {
        sfxMixer.SetFloat("sfxVol", Mathf.Log10(sliderValue) * 20);
    }
}
