using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SongAudioSource : MonoBehaviour
{
    [SerializeField] private int _idProject;
    public static SongAudioSource Instance { get; private set; }
    public Dictionary<string, AudioClip> AudioDictionary = new();
    private AudioSource _musicSource;

    private Coroutine _loadingClip;
    private AudioClip _currentClip;
    private bool _isLoadingSong = false;

    public AudioSource MusicSource => _musicSource;
    public bool IsLoading => _isLoadingSong;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        _musicSource = GetComponent<AudioSource>();

        StartCoroutine(LoadingClip(AudioKeys.Coin.ToString(), clip => { }));
    }

    public void PlayMusic()
    {
        _musicSource.clip = _currentClip;
        _musicSource.Play();
    }

    public void PlayOneShot(string idAudio)
    {
        _musicSource.PlayOneShot(AudioDictionary[idAudio]);
    }

    public void LoadClip(string idAudio)
    {
        if (_loadingClip == null)
            _loadingClip = StartCoroutine(LoadingClip(idAudio, clip =>
            {
                _loadingClip = null; _currentClip = clip; _isLoadingSong = false;
            }));
    }

    private IEnumerator LoadingClip(string idAudio, Action<AudioClip> callback)
    {
        _isLoadingSong = true;
        _musicSource.Stop();

        if (AudioDictionary.TryGetValue(idAudio, out AudioClip audioClip))
        {
            callback?.Invoke(audioClip);
        }
        else
        {
            using UnityWebRequest request =
                UnityWebRequestMultimedia.GetAudioClip(
                    $"https://s3.eponesh.com/games/files/{_idProject}/{idAudio}.mp3", AudioType.MPEG);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                AudioDictionary[idAudio] = clip;
                callback?.Invoke(clip);
            }
            else
            {
                Debug.LogWarning($"Failed to load audio clip '{idAudio}': {request.error}");
                callback?.Invoke(null);
            }
        }

        _isLoadingSong = false;
    }
}

public enum AudioKeys
{
    Years,
    Faded,
    Attention,
    Adcdefu,
    BabyShark,
    BadGuy,
    ThisIsWhatYouCameFor,
    Havana,
    WeDontTalkAnymore,
    Enemy,
    SomeWhereOnlyWeKnow,
    KingsQueens,
    OneMoreNight,
    SeeYouAgain,
    WakaWaka,
    ShapeOfYou,
    Stay,
    TheGummyBearSong,
    BlindingLights,
    DanceMonkey,
    Coin,
}