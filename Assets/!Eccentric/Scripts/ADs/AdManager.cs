using System;
using System.Collections;
using System.Collections.Generic;
using Eccentric;
using UnityEngine;
using GamePush;
using Platform = Eccentric.Platform;

public abstract class AdManager 
{
    protected readonly Platform _platform;
    public static bool IsShowing;
    public static bool IsRewardAdReady;
    protected Action _onReward;
    protected float _timeScale;
    protected bool _isPauseAudioListener;
    protected bool _isCashed;
    protected float _timerForAds;
    protected readonly float _limitForTimer = 180f;

    public AdManager(Platform platform)
    {
        _platform = platform;
        _timeScale = Time.timeScale;
        _isPauseAudioListener = AudioListener.pause;
        _isCashed = true;
        _timerForAds = 0;
    }

    public abstract void SwitchStickyBanner( bool isAvailable);
    public abstract void CashAd();
    public abstract void ShowAd();
    public abstract void ShowRewardAd(Action onReward);
    public abstract void Subscribe();
    public abstract void Unsubscribe();
    protected abstract void OnRewardedVideoShowed();
    protected abstract void OnPreloadRewardedVideoHandler(int obj);
    protected abstract void OnRewardAdReadyHandler();
    protected abstract void CheckRewardAd();
    protected abstract void OnRewardAdFailure();
    public abstract IEnumerator CheckRewardAdCoroutine();
    public abstract IEnumerator PauseOnStartCoroutine();
    protected abstract void OnRewardGameHandler();
    public abstract void PreloadRewardAd();
    public abstract void CacheInterstitial();
    public abstract void CacheRewarded();
    protected abstract void OnRewardStatusHandler(bool available);
  
    
    public bool IsCanShowSticky()
    {
        return _platform switch
        {
            Platform.VK or
                Platform.OK or
                Platform.GAME_DISTRIBUTION or
                Platform.YANDEX => true,
            _ => false,
        };
    }


    
    public IEnumerator UpdateTimerCoroutine()
    {
        while (_timerForAds < _limitForTimer)
        {
            _timerForAds += Time.deltaTime;
            yield return null;
          
        }
    }
    
    protected void PauseGameplay()
    {
        Debug.LogWarning("PauseGameplay");
        if (!_isCashed)
        {
            _timeScale = Time.timeScale;
            _isPauseAudioListener = AudioListener.pause;
            _isCashed = true;
        }

        IsShowing = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }
    protected void ResumeGameplay(bool arg)
    {
        Debug.LogWarning("ResumeGameplay");
        IsShowing = false;
        Time.timeScale = _timeScale;
        AudioListener.pause = _isPauseAudioListener;
        _isCashed = false;
    }

    protected void ResumeGameplay()
    {
        Debug.LogWarning("ResumeGameplay");
        IsShowing = false;
        Time.timeScale = _timeScale;
        AudioListener.pause = _isPauseAudioListener;
    }
    

   


}
