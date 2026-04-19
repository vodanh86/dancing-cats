using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Events;
using System;

namespace com.jiogames.wrapper
{
    public class JioWrapperJS : MonoBehaviour
    {
        internal static JioWrapperJS Instance { get; private set; }

        [DllImport("__Internal")]
        private static extern void PostScore(int score);
        [DllImport("__Internal")]
        private static extern void CacheInterstitial(string adKeyId, string package);
        [DllImport("__Internal")]
        private static extern void ShowInterstitial(string adKeyId, string package);
        [DllImport("__Internal")]
        private static extern void CacheRewarded(string adKeyId, string package);
        [DllImport("__Internal")]
        private static extern void ShowRewarded(string adKeyId, string package);
        [DllImport("__Internal")]
        private static extern void GetUserProfile();


        [SerializeField] internal string adSpotInterstitial = "zbjnq9gs";
        [SerializeField] internal string adSpotRewardedVideo = "81xnt9bw";
        [SerializeField] internal string packageName = "com.jiogames.testsp";

        internal bool IsAdReady {get; private set;}
        internal bool IsRVReady  {get; private set;}
        internal bool IsRewardUser  {get; private set;}

        UserProfileInfo profileInfo;
        internal Detail ProfileInfo { get { return profileInfo.detail; } }

        public static event Action JioRewarded;
        public static event Action<bool> OnAdRewardChangeStatus;

        // void Awake(){
        //     if (Instance != null && Instance != this) 
        //     { 
        //         Destroy(this); 
        //     } 
        //     else 
        //     { 
        //         Instance = this; 
        //         DontDestroyOnLoad(gameObject);
        //     } 
        // 
        // }
        public void Init()
        {
            Debug.Log("JioGamesJS: SDK initialize : 1.0.0");
            Instance = this;
        }

        #region "Methods"
        internal void getUserProfile() {
            #if !UNITY_EDITOR
                GetUserProfile();
            #endif
        }

        internal void postScore(int score) {
            #if !UNITY_EDITOR
                PostScore(score);
            #else
                Debug.Log($"JioGamesJS: PostScore : {score}");
            #endif
        }

        internal void cacheInterstitial() {
            if (!IsAdReady) {
                #if !UNITY_EDITOR
                    CacheInterstitial(adSpotInterstitial, packageName);
                #else
                    onAdPrepared(adSpotInterstitial);
                #endif
            }
        }
        internal void cacheRewarded() {
            if (!IsRVReady) {
                #if !UNITY_EDITOR
                    CacheRewarded(adSpotRewardedVideo, packageName);
                #else
                    onAdPrepared(adSpotRewardedVideo);
                #endif
            }    
        }
        internal void showInterstitial() {
            Debug.Log("IsAdReady: " +IsAdReady);
            if (IsAdReady) {
                #if !UNITY_EDITOR
                    ShowInterstitial(adSpotInterstitial, packageName);
                #else
                    onAdClosed(adSpotInterstitial+"|false|false");
                #endif
            }
        }
        internal void showRewarded() {
            Debug.Log("IsRVReady: " +IsRVReady);
            if (IsRVReady) {
                #if !UNITY_EDITOR
                    IsRewardUser=false;
Debug.Log($"ShowRewarded: {IsRVReady}, adSpotRewardedVideo: {adSpotRewardedVideo},packageName {packageName} ");
                    ShowRewarded(adSpotRewardedVideo, packageName);
                #else
                    onAdClosed(adSpotRewardedVideo+"|true|true");
                #endif
            }
        }

        // When call the both cacheAd at same time then use this method.
        internal void cacheAd(){
            StartCoroutine(CacheAd());
        }
        private IEnumerator CacheAd()
        {
            cacheInterstitial();
            yield return new WaitForSeconds(5);
            cacheRewarded();
        }
        #endregion

        #region "Callbacks"
        void onAdPrepared(string adSpotKey){
            if(string.Equals(adSpotKey, adSpotInterstitial)){
                IsAdReady = true;
                Debug.Log("JioGamesJS: onAdPrepared MidRoll " + IsAdReady);
            }
            else if(string.Equals(adSpotKey, adSpotRewardedVideo)){
                IsRVReady = true;
                OnAdRewardChangeStatus?.Invoke(IsRVReady);
                Debug.Log("JioGamesJS: onAdPrepared RewardedVideo " + IsRVReady);
            }
            else { }
        }
        void onAdClosed(string localData){
            Debug.Log("JioGamesJS: onAdClosed localData : " + localData);

            string[] resData = localData.Split('|');
            string adSpotKey = resData[0];
            bool pIsVideoCompleted = bool.Parse(resData[1]);
            bool pIsEligibleForReward = bool.Parse(resData[2]);

            if(string.Equals(adSpotKey, adSpotInterstitial)){
                IsAdReady = false;
                //cacheInterstitial();
                Debug.Log("JioGamesJS: onAdClose MidRoll " + IsAdReady);
            }
            else if(string.Equals(adSpotKey, adSpotRewardedVideo)){
                IsRVReady = false;
                OnAdRewardChangeStatus?.Invoke(IsRVReady);
                //cacheRewarded();
                Debug.Log("JioGamesJS: onAdClose RewardedVideo " + IsRVReady);

                if (pIsVideoCompleted) {
                    OnReward();
                    IsRewardUser = pIsEligibleForReward;
                }
            }
            else { }
        }
        void onAdFailedToLoad(string localData){
            Debug.Log("JioGamesJS: onAdFailedToLoad localData : " + localData);

            string[] resData = localData.Split('|');
            string adSpotKey = resData[0];

            if(string.Equals(adSpotKey, adSpotInterstitial)){
                IsAdReady = false;
                Debug.Log("JioGamesJS: onAdFailedToLoad MidRoll " + IsAdReady);
                //cacheInterstitial();
            }
            else if(string.Equals(adSpotKey, adSpotRewardedVideo)){
                IsRVReady = false;
                OnAdRewardChangeStatus?.Invoke(IsRVReady);
                Debug.Log("JioGamesJS: onAdFailedToLoad RewardedVideo " + IsRVReady);
                //cacheRewarded();
            }
            else { }
        }

        void onUserProfileResponse(string userInfo){
            Debug.Log("JioGamesJS: onUserProfileResponse Info : " + userInfo);

            profileInfo = JsonUtility.FromJson<UserProfileInfo>(userInfo);
            Debug.Log(ProfileInfo.gamer_id);
            Debug.Log(ProfileInfo.gamer_name);
            Debug.Log(ProfileInfo.gamer_avatar_url);
            Debug.Log(ProfileInfo.device_type);
            Debug.Log(ProfileInfo.dob);
        }
        #endregion

        void OnReward(){
            Debug.Log("JioGamesJS : OnRewardJio : Event Called...");
            JioRewarded?.Invoke();
         
        }

        #region "Forground and Background"

        void onClientPause(){
            Debug.Log("JioGamesJS : onClientPause");
        }
        void onClientResume(){
            Debug.Log("JioGamesJS : onClientResume");
        }
        void ResumeGameSound(){
            Debug.Log("JioGamesJS : ResumeGameSound");
            AudioListener.pause = false;
          //  Jio.SampleGame.AudioManager.Instance.PauseResume(false);
        }
        void PauseGameSound(){
            Debug.Log("JioGamesJS : PauseGameSound");
            AudioListener.pause = true;
            // Jio.SampleGame.AudioManager.Instance.PauseResume(true);
        }


        #endregion
    }

    [Serializable]
    public class Detail
    {
        public string gamer_id;
        public string gamer_name;
        public string gamer_avatar_url;
        public string device_type;
        public string dob;
    }
    [Serializable]
    public class UserProfileInfo
    {
        public Detail detail;
    }
}