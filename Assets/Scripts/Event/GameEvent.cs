using Eccentric;
using GamePush;
using System;
using System.Collections;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public static GameEvent Instance { get; private set; }

    [SerializeField] private GameEventType _gameEventType;
    [SerializeField] private int _targetPlayTime = 1500;
    [Space]
    [SerializeField] private GameEventNotificationPopup _notificationPopup;
    [SerializeField] private GameEventRewardPopup _rewardPopup;
    [Space]
    [SerializeField] private int _skinID;

    private GameEventData _gameEventData;
    private Shop _shop;

    private bool _isNotificationShowed;
    private bool _isNeenToShowReward;

    public GameEventData GameEventData => _gameEventData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        _gameEventData = new GameEventData(_targetPlayTime, _gameEventType);
    }


    private IEnumerator Start()
    {
        yield return new WaitUntil(() => SaveSystemWithData.PlayerData != null);
        _gameEventData.Load();

        yield return new WaitUntil(() => LevelLoader.Instance != null);
        LevelLoader.Instance.LevelLoaded += OnLevelLoaded;
    }

    private void OnLevelLoaded()
    {
        if (!_isNotificationShowed)
        {
            if (!_gameEventData.GotSkin)
            {
                _notificationPopup.Open();
                _isNotificationShowed = true;
            }
        }

        if (_isNeenToShowReward)
        {
            _rewardPopup.Open();
            _isNeenToShowReward = false;

            Invoke(nameof(ForceActivateSkin), 0.7f);
        }
    }

    public void OpenMenu()
    {
        if (_gameEventData.GotSkin)
            _rewardPopup.Open();
        else
            _notificationPopup.Open();
    }

    public void SetShop(Shop shop)
    {
        _shop = shop;
    }

    public void AddPlayedTime(int time)
    {
        _gameEventData.AddTime(time);

        if (_gameEventData.IsTimeLeft)
        {
            _isNeenToShowReward = true;
            _gameEventData.GetSkin();
            GiveReward();
        }
    }

    private void GiveReward()
    {
        SaveSystemWithData.PlayerData.SkinsID.Add(_skinID);
        EccentricInit.Instance.SaveSystemWithData.Save(SaveSystemWithData.PlayerData, true);
        GP_Analytics.Goal("EVENT_SKIN_EARN", 0);
    }

    private void ForceActivateSkin()
    {
        _shop.ForceActivateSkin(_skinID);
    }
}

[Serializable]
public enum GameEventType
{
    SummerFest,
}
