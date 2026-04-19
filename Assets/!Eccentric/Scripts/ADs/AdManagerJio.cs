using System;
using System.Collections;
using System.Collections.Generic;
using com.jiogames.wrapper;
using Eccentric;
using UnityEngine;

public class AdManagerJio : AdManager
{
    public AdManagerJio(Platform platform) : base(platform)
    {
    }

    public override void SwitchStickyBanner(bool isAvailable)
    {
        
    }

    public override void CashAd()
    {
        JioWrapperJS.Instance.cacheInterstitial();
        JioWrapperJS.Instance.cacheRewarded();
    }

    public override void ShowAd()
    {
        if (IsShowing) return;
        JioWrapperJS.Instance.showInterstitial();
    }

    public override void ShowRewardAd(Action onReward)
    {
        if (IsShowing) return;
        _onReward = onReward;
        JioWrapperJS.Instance.showRewarded();
    }

    public override void Subscribe()
    {
        JioWrapperJS.JioRewarded += OnRewardGameHandler;
        JioWrapperJS.OnAdRewardChangeStatus += OnRewardStatusHandler;
    }

    public override void Unsubscribe()
    {
        JioWrapperJS.JioRewarded -= OnRewardGameHandler;
        JioWrapperJS.OnAdRewardChangeStatus -= OnRewardStatusHandler;
    }

    protected override void OnRewardedVideoShowed()
    {
    }

    protected override void OnPreloadRewardedVideoHandler(int obj)
    {
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
    }

    public override void CacheInterstitial()
    {
        JioWrapperJS.Instance.cacheInterstitial();
    }

    public override void CacheRewarded()
    {
        JioWrapperJS.Instance.cacheRewarded();
    }

    protected override void OnRewardStatusHandler(bool available)
    {
        IsRewardAdReady = available;
    }
}