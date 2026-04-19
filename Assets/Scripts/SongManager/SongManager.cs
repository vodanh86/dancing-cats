using Unity.VisualScripting;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    [SerializeField] private float _songBpm;
    [SerializeField] private float _firstBeatOffset;

    private float _beatPerSec;
    private int _currentSample = 0;
    private bool _isGameStopped = false;

    public static SongManager Instance;
    public static float Speed;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        _beatPerSec = _songBpm / 60;

        Speed = _beatPerSec * 2f;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (_isGameStopped)
            return;

        if (GameStopper.Instance == null || !GameStopper.Instance.IsPlayingSong)
            return;

        if (focus)
        {
            if (GameStopper.Instance.IsCounting)
            {
                GameStopper.Instance.StartCountingDown(() =>
                {
                    Time.timeScale = 1f;
                    AudioListener.pause = false;
                    SongAudioSource.Instance.MusicSource.Play();
                    SongAudioSource.Instance.MusicSource.timeSamples = _currentSample;
                });
            }
            else
            {
                GameStopper.Instance.RestartCountingDown(() =>
                {
                    Time.timeScale = 1f;
                    AudioListener.pause = false;
                    SongAudioSource.Instance.MusicSource.Play();
                    SongAudioSource.Instance.MusicSource.timeSamples = _currentSample;
                });
            }

        }
        else
        {
            if (!GameStopper.Instance.IsGameStoped)
            {
                GameStopper.Instance.SetStopGameStatus();
                _currentSample = SongAudioSource.Instance.MusicSource.timeSamples;
                SongAudioSource.Instance.MusicSource.Pause();
                AudioListener.pause = true;
                Time.timeScale = 0f;
            }
        }
    }

    public void StartMusic()
    {
        AudioListener.pause = false;
        SongAudioSource.Instance.MusicSource.timeSamples = _currentSample;
        SongAudioSource.Instance.PlayMusic();
    }

    public void PauseMusic()
    {
        _isGameStopped = true;
        _currentSample = SongAudioSource.Instance.MusicSource.timeSamples;
        SongAudioSource.Instance.MusicSource.Pause();
        AudioListener.pause = true;
    }

    public void UnpauseMusic()
    {
        AudioListener.pause = false;
        SongAudioSource.Instance.MusicSource.Play();
        SongAudioSource.Instance.MusicSource.timeSamples = _currentSample;
        _isGameStopped = false;
    }

    public void StopMusic()
    {
        SongAudioSource.Instance.MusicSource.Stop();
    }
}