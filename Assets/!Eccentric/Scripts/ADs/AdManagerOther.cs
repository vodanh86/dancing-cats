using System;
using System.Collections;
using System.Collections.Generic;
using GamePush;
using UnityEngine;
using Platform = Eccentric.Platform;

public class AdManagerOther : AdManager
{
    public AdManagerOther(Platform platform) : base(platform)
    {
    }

    public override void SwitchStickyBanner(bool isAvailable)
    {
        if (isAvailable)
        {
            GP_Ads.ShowSticky();
        }
    }

    public override void CashAd()
    {
       
    }

    public override void ShowAd()
    {
        if (IsShowing) return;
        if (_platform == Platform.CRAZY_GAMES && _timerForAds < _limitForTimer) return;
        GP_Ads.ShowFullscreen(PauseGameplay, ResumeGameplay);
    }

    public override void ShowRewardAd(Action onReward)
    {
        if (IsShowing) return;
            GP_Ads.ShowRewarded(onRewardedReward: (s) => onReward(), onRewardedStart: PauseGameplay,
                onRewardedClose: ResumeGameplay);

        
    }

    public override void Subscribe()
    {
        GP_Ads.OnPreloaderStart += PauseGameplay;
        GP_Ads.OnPreloaderClose += ResumeGameplay;
    }

    public override void Unsubscribe()
    {
        GP_Ads.OnPreloaderStart -= PauseGameplay;
        GP_Ads.OnPreloaderClose -= ResumeGameplay;
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
        PauseGameplay();
        yield return new WaitWhile(GP_Ads.IsPreloaderPlaying);
        ResumeGameplay();
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
     
    }

    public override void CacheRewarded()
    {
   
    }

    protected override void OnRewardStatusHandler(bool available)
    {
        
    }
}
