using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameDistribution : MonoBehaviour
{
    public static GameDistribution Instance;

    public string GAME_KEY = "YOUR_GAME_KEY_HERE";

    public static Action OnReadySDK;
    public static Action OnResumeGame;
    public static Action OnPauseGame;
    public static Action OnRewardGame;
    public static Action OnRewardedVideoSuccess;
    public static Action OnRewardedVideoFailure;
    public static Action<int> OnPreloadRewardedVideo;

    [DllImport("__Internal")]
    private static extern void SDK_Init_GD(string gameKey);

    [DllImport("__Internal")]
    private static extern void SDK_PreloadAd_GD();

    [DllImport("__Internal")]
    private static extern void SDK_ShowAd_GD(string adType);

    [DllImport("__Internal")]
    private static extern void SDK_SendEvent_GD(string options);

    private bool _isRewardedVideoLoaded = false;

    void Awake()
    {
        Instance ??= this;
    }

    internal void Init(string gameKey)
    {
        GAME_KEY = gameKey;
        try
        {
            SDK_Init_GD(GAME_KEY);
        }
        catch (EntryPointNotFoundException e)
        {
            Debug.LogWarning("GD initialization failed. Make sure you are running a WebGL build in a browser:" +
                             e.Message);
        }
    }

    internal void ShowAd()
    {
        try
        {
            SDK_ShowAd_GD(null);
        }
        catch (EntryPointNotFoundException e)
        {
            Debug.LogWarning("GD ShowAd failed. Make sure you are running a WebGL build in a browser:" + e.Message);
        }
    }

    internal void ShowRewardedAd()
    {
        try
        {
            SDK_ShowAd_GD("rewarded");
        }
        catch (EntryPointNotFoundException e)
        {
            Debug.LogWarning("GD ShowAd failed. Make sure you are running a WebGL build in a browser:" + e.Message);
        }
    }

    internal void PreloadRewardedAd()
    {
        try
        {
            SDK_PreloadAd_GD();
        }
        catch (EntryPointNotFoundException e)
        {
            Debug.LogWarning("GD Preload failed. Make sure you are running a WebGL build in a browser:" + e.Message);
        }
    }

    internal void SendEvent(string options)
    {
        try
        {
            SDK_SendEvent_GD(options);
        }
        catch (EntryPointNotFoundException e)
        {
            Debug.LogWarning("GD SendEvent failed. Make sure you are running a WebGL build in a browser:" + e.Message);
        }
    }

    void OnReadySDKCallback()
    {
        OnReadySDK?.Invoke();
    }

    /// <summary>
    /// It is being called by HTML5 SDK when the game should start.
    /// </summary>
    void ResumeGameCallback()
    {
        if (OnResumeGame != null) OnResumeGame();
    }

    /// <summary>
    /// It is being called by HTML5 SDK when the game should pause.
    /// </summary>
    void PauseGameCallback()
    {
        if (OnPauseGame != null) OnPauseGame();
    }

    /// <summary>
    /// It is being called by HTML5 SDK when the game should should give reward.
    /// </summary>
    void RewardedCompleteCallback()
    {
        if (OnRewardGame != null) OnRewardGame();
    }

    /// <summary>
    /// It is being called by HTML5 SDK when the rewarded video succeeded.
    /// </summary>
    void RewardedVideoSuccessCallback()
    {
        _isRewardedVideoLoaded = false;

        if (OnRewardedVideoSuccess != null) OnRewardedVideoSuccess();
    }

    /// <summary>
    /// It is being called by HTML5 SDK when the rewarded video failed.
    /// </summary>
    void RewardedVideoFailureCallback()
    {
        _isRewardedVideoLoaded = false;

        if (OnRewardedVideoFailure != null) OnRewardedVideoFailure();
    }

    /// <summary>
    /// It is being called by HTML5 SDK when it preloaded rewarded video
    /// </summary>
    void PreloadRewardedVideoCallback(int loaded)
    {
        _isRewardedVideoLoaded = (loaded == 1);

        if (OnPreloadRewardedVideo != null) OnPreloadRewardedVideo(loaded);
    }

    public bool IsRewardedVideoLoaded()
    {
        return _isRewardedVideoLoaded;
    }
}