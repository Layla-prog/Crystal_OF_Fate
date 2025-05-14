using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource mainAudio;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = volume;
        mainAudio.volume = volume;
    }

    public void OnVolumeChanged(float value)
    {
        mainAudio.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }
}
