using System;
using System.Collections;
using System.Collections.Generic;
using Eccentric;
using UnityEngine;

public class AdManagerGamedistribution : AdManager
{
    public AdManagerGamedistribution(Platform platform) : base(platform)
    {
    }

    public override void SwitchStickyBanner(bool isAvailable)
    {
        
    }

    public override void CashAd()
    {
        
    }

    public override void ShowAd()
    {
        if (IsShowing) return;
        GameDistribution.Instance.ShowAd();
    }

    public override void ShowRewardAd(Action onReward)
    {
        if (IsShowing) return;
        _onReward = onReward;
        if (GameDistribution.Instance.IsRewardedVideoLoaded())
        {
            GameDistribution.Instance.ShowRewardedAd();
        }
        else
        {
            GameDistribution.Instance.PreloadRewardedAd();
        }
    }

    public override void Subscribe()
    {
        PreloadRewardAd();
        GameDistribution.OnPauseGame += PauseGameplay;
        GameDistribution.OnResumeGame += ResumeGameplay;
        GameDistribution.OnRewardGame += OnRewardGameHandler;
        GameDistribution.OnPreloadRewardedVideo += OnPreloadRewardedVideoHandler;
        GameDistribution.OnRewardedVideoSuccess += OnRewardedVideoShowed;
        GameDistribution.OnRewardedVideoFailure += OnRewardedVideoShowed;
    }

    public override void Unsubscribe()
    {
        GameDistribution.OnPauseGame -= PauseGameplay;
        GameDistribution.OnResumeGame -= ResumeGameplay;
        GameDistribution.OnRewardGame -= OnRewardGameHandler;
        GameDistribution.OnPreloadRewardedVideo -= OnPreloadRewardedVideoHandler;
        GameDistribution.OnRewardedVideoSuccess -= OnRewardedVideoShowed;
        GameDistribution.OnRewardedVideoFailure -= OnRewardedVideoShowed;
    }

    protected override void OnRewardedVideoShowed()
    {
        GameDistribution.Instance.PreloadRewardedAd();
    }

    protected override void OnPreloadRewardedVideoHandler(int obj)
    {
        if (obj == 1) return;
        GameDistribution.Instance.PreloadRewardedAd();
    }

    protected override void OnRewardAdReadyHandler()
    {
        
    }

    protected override void CheckRewardAd()
    {
        
    }

    protected override void OnRewardAdFailure()
    {
        
    }

    public override IEnumerator CheckRewardAdCoroutine()
    {
        yield break;
    }

    public override IEnumerator PauseOnStartCoroutine()
    {
        yield break;
    }

    protected override void OnRewardGameHandler()
    {
        Debug.LogWarning("OnRewardGameHandler");
        _onReward?.Invoke();
    }

    public override void PreloadRewardAd()
    {
        GameDistribution.Instance.PreloadRewardedAd();
    }

    public override void CacheInterstitial()
    {
     
    }

    public override void CacheRewarded()
    {
        
    }

    protected override void OnRewardStatusHandler(bool available)
    {
       
    }
}
