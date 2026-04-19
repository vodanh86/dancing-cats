using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Eccentric;
using UnityEngine;

public class LaggedAPIUnity : MonoBehaviour
{

    public static LaggedAPIUnity Instance;

    public string DEV_ID = "YOUR_DEV_ID";
    public string PUBLISHER_ID = "YOUR_PUBLISHER_ID";

    
    public static Action OnResumeGame;
    public static Action OnPauseGame;
    public static Action onRewardAdReady;
    public static Action onRewardAdSuccess;
    public static Action onRewardAdFailure;

    private static bool _isLoaded;

    [DllImport("__Internal")]
    private static extern void SDK_Init_LAGGED(string devId, string publisherId);

    [DllImport("__Internal")]
    private static extern void SDK_CallHighScore_LAGGED(int SCORE, string BOARD_ID);

    [DllImport("__Internal")]
    private static extern void SDK_SaveAchievement_LAGGED(string AWARD);

    [DllImport("__Internal")]
    private static extern void SDK_ShowAd_LAGGED();

    [DllImport("__Internal")]
    private static extern void SDK_CheckRewardAd_LAGGED();

    [DllImport("__Internal")]
    private static extern void SDK_PlayRewardAd_LAGGED();
    
    [DllImport("__Internal")] 
    private static extern void SDK_Init_LAGGED_Without_Adsense();

    // void Awake()
    // {
    //     if (LaggedAPIUnity.Instance == null)
    //         LaggedAPIUnity.Instance = this;
    //     else
    //         Destroy(this);
    //
    //     DontDestroyOnLoad(this);
    //     
    // }

    internal  void Init()
    {
 
        try
        {
            SDK_Init_LAGGED(DEV_ID, PUBLISHER_ID);
        }
        catch (EntryPointNotFoundException e)
        {
           // Debug.LogWarning("LaggedAPI initialization failed. Make sure you are running a WebGL build in a browser:" + e.Message);
        }
    }
    internal  void InitWithoutAdsense()
    {
     
        try
        {
            SDK_Init_LAGGED("lagdev_2", "ca-pub-8999528956543364");
        }
        catch (EntryPointNotFoundException e)
        {
            // Debug.LogWarning("LaggedAPI initialization failed. Make sure you are running a WebGL build in a browser:" + e.Message);
        }
    }
    internal void ShowAd()
    {
        if(!_isLoaded) return;
        try
        {
            SDK_ShowAd_LAGGED();
        }
        catch (EntryPointNotFoundException e)
        {
           // Debug.LogWarning("LaggedAPI ShowAd failed. Make sure you are running a WebGL build in a browser:" + e.Message);
        }
    }

    internal void CheckRewardAd()
    {
        if(!_isLoaded) return;
        try
        {
            SDK_CheckRewardAd_LAGGED();
        }
        catch (EntryPointNotFoundException e)
        {
           // Debug.LogWarning("LaggedAPI Reward Ad failed. Make sure you are running a WebGL build in a browser:" + e.Message);
        }
    }

    internal void PlayRewardAd()
    {
        if(!_isLoaded) return;
        try
        {
            SDK_PlayRewardAd_LAGGED();
        }
        catch (EntryPointNotFoundException e)
        {
           // Debug.LogWarning("LaggedAPI Reward Ad failed. Make sure you are running a WebGL build in a browser:" + e.Message);
        }
    }
    

    internal void CallHighScore(int score, string board)
    {
        if(!_isLoaded) return;
        try
        {
            SDK_CallHighScore_LAGGED(score,board);
        }
        catch (EntryPointNotFoundException e)
        {
           // Debug.LogWarning("LaggedAPI Call High Score failed: " + e.Message);
        }
    }

    internal void SaveAchievement(string award)
    {
        if(!_isLoaded) return;
        try
        {
            SDK_SaveAchievement_LAGGED(award);
        }
        catch (EntryPointNotFoundException e)
        {
          //  Debug.LogWarning("LaggedAPI Save Achievement failed: " + e.Message);
        }
    }
    void OnSdkInitCallback()
    {
        _isLoaded = true;
    }

    ///
    /// Called when ad is completed and the game should start.
    ///
    void ResumeGameCallback()
    {
        if (OnResumeGame != null) OnResumeGame();
    }

    ///
    /// Called when ad starts, game/music should pause
    ///
    void PauseGameCallback()
    {
        if (OnPauseGame != null) OnPauseGame();
    }

    ///
    /// if reward ad is ready
    ///
    void RewardAdReadyCallback()
    {
        if (onRewardAdReady != null) onRewardAdReady();
    }

    ///
    /// if reward is successful, give player reward
    ///
    void RewardAdSuccessCallback()
    {
        if (onRewardAdSuccess != null) onRewardAdSuccess();
    }

    ///
    /// if reward ad failed, no reward
    ///
    void RewardAdFailCallback()
    {
        if (onRewardAdFailure != null) onRewardAdFailure();
    }

}
