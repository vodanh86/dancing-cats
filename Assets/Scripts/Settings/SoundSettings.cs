using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private AudioMixer _mixer;

    private const string SoundValue = nameof(SoundValue);
    private const string Master = nameof(Master);

    private void OnEnable()
    {
        _soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
    }

    private void OnDisable()
    {
        _soundSlider.onValueChanged.RemoveListener(OnSoundSliderChanged);
    }

    private void Start()
    {
        _soundSlider.value = PlayerPrefs.GetFloat(SoundValue, 0);
    }

    private void OnSoundSliderChanged(float value)
    {
        _mixer.SetFloat(Master, value);

        PlayerPrefs.SetFloat(SoundValue, value);
    }
}