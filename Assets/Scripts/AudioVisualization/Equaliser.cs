using UnityEngine;
using UnityEngine.UI;

public class Equaliser : MonoBehaviour
{
    [SerializeField] private Slider[] _sliders;

    private void OnEnable()
    {
        AudioPeer.Instance.AudioBandsChange += OnAudioBandsChange;
    }

    private void OnDisable()
    {
        AudioPeer.Instance.AudioBandsChange -= OnAudioBandsChange;
    }

    private void OnAudioBandsChange(float[] bands)
    {
        for (int i = 0; i < bands.Length; i++)
        {
            _sliders[i].value = bands[i];
        }
    }
}
