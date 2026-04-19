using System;
using System.Collections;
using System.Collections.Generic;
using Eccentric;
using UnityEngine;

public class AdManagerLagged : AdManager
{
    public AdManagerLagged(Platform platform) : base(platform)
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
        LaggedAPIUnity.Instance.ShowAd();
    }

    public override void ShowRewardAd(Action onReward)
    {
        if (IsShowing) return;
        _onReward = onReward;
        LaggedAPIUnity.Instance.PlayRewardAd();
    }

    public override void Subscribe()
    {
        LaggedAPIUnity.OnPauseGame += PauseGameplay;
        LaggedAPIUnity.OnResumeGame += ResumeGameplay;
        LaggedAPIUnity.onRewardAdReady += OnRewardAdReadyHandler;
        LaggedAPIUnity.onRewardAdSuccess += OnRewardGameHandler;
        LaggedAPIUnity.onRewardAdFailure += OnRewardAdFailure;
    }

    public override void Unsubscribe()
    {
        LaggedAPIUnity.OnPauseGame -= PauseGameplay;
        LaggedAPIUnity.OnResumeGame -= ResumeGameplay;
        LaggedAPIUnity.onRewardAdReady -= OnRewardAdReadyHandler;
        LaggedAPIUnity.onRewardAdSuccess -= OnRewardGameHandler;
        LaggedAPIUnity.onRewardAdFailure -= OnRewardAdFailure;
    }

    protected override void OnRewardedVideoShowed()
    {
    
    }

    protected override void OnPreloadRewardedVideoHandler(int obj)
    {
   
    }

    protected override void OnRewardAdReadyHandler()
    {
        IsRewardAdReady = true;
    }

    protected override void CheckRewardAd()
    {
        if (IsRewardAdReady) return;
        LaggedAPIUnity.Instance.CheckRewardAd();
        Debug.LogWarning("CheckRewardAd");
    }

    protected override void OnRewardAdFailure()
    {
        IsRewardAdReady = false;
    }

    public override IEnumerator CheckRewardAdCoroutine()
    {
        while (true)
        {
            yield return new WaitWhile(() => IsRewardAdReady);
            yield return new WaitForSeconds(3);
            CheckRewardAd();
        }
    }

    public override IEnumerator PauseOnStartCoroutine()
    {
        yield break;
    }

    protected override void OnRewardGameHandler()
    {
        Debug.LogWarning("OnRewardGameHandler");
        _onReward?.Invoke(); 
        IsRewardAdReady = false;

    }

    public override void PreloadRewardAd()
    {
 
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
