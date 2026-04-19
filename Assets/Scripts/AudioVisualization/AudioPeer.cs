using System;
using UnityEngine;

public sealed class AudioPeer : MonoBehaviour
{
    public static AudioPeer Instance { get; private set; }

    [SerializeField] private AudioSource _audioSource;

    private float[] _samples = new float[_samplesCount];
    private float[] _bandFrequencies = new float[_bandsCount];
    private float[] _bandBuffers = new float[_bandsCount];
    private float[] _bufferDecreases = new float[_bandsCount];
    private float[] _bandBufferHighestValues = new float[_bandsCount];
    private float[] _audioBands = new float[_bandsCount];

    private const int _samplesCount = 512;
    private const int _bandsCount = 8;
    private const int _bandsMultiplier = 10;
    private const float _decreaseStartValue = 0.005f;
    private const float _decreaseMultiplier = 1.2f;

    public Action<float[]> AudioBandsChange;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!_audioSource.isPlaying)
            return;

        GetSpectrumAudioSource();
        MadeFrequencyBands();
        SetBandBuffers();
        CreateAudioBands();
    }

    private void GetSpectrumAudioSource()
    {
        _audioSource.clip.GetData(_samples, _audioSource.timeSamples);
    }

    private void MadeFrequencyBands()
    {
        int count = 0;
        float average;

        for (int i = 0; i < _bandsCount; i++)
        {
            _bandFrequencies[i] = 0;
            average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == _bandsCount - 1)
                sampleCount += 2;

            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                count++;
            }

            average /= count;

            _bandFrequencies[i] = average * _bandsMultiplier;
        }
    }

    private void SetBandBuffers()
    {
        for (int i = 0; i < _bandsCount; i++)
        {
            if (_bandFrequencies[i] >= _bandBuffers[i])
            {
                _bandBuffers[i] = _bandFrequencies[i];
                _bufferDecreases[i] = _decreaseStartValue;
            }
            else
            {
                _bandBuffers[i] -= _bufferDecreases[i];
                _bufferDecreases[i] *= _decreaseMultiplier;
            }
        }
    }

    private void CreateAudioBands()
    {
        for (int i = 0; i < _bandsCount; i++)
        {
            if (_bandBuffers[i] > _bandBufferHighestValues[i])
                _bandBufferHighestValues[i] = _bandBuffers[i];

            _audioBands[i] = _bandBuffers[i] / _bandBufferHighestValues[i];
        }

        AudioBandsChange?.Invoke(_audioBands);
    }
}
