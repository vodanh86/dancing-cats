using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System;
using Eccentric;

public class RouletteRewarder : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _rewardButton;
    [SerializeField] private Button _noThanksButton;
    [SerializeField] private TMP_Text _rewardMoneyText;
    [Space]
    [SerializeField] private TMP_Text _currentMultiplierText;
    [Space]
    [Header("CycleTime")]
    [SerializeField] private float _oneCycleTime = 2f;

    private int _reward = 0;
    private Coroutine _spiningRoulette;
    private Coroutine _hidingSkipButton;
    private Coroutine _waitingBeforeSendEvent;
    private float _delayBeforeSendEvent = 2f;

    private float _currentMultiplier = 1;

    public event Action RewardTakenOrSkipped;

    private void Awake()
    {
        _noThanksButton.transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        _rewardButton.onClick.AddListener(OnButtonClick);
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
        _noThanksButton.onClick.AddListener(InterAndSendEvent);
    }

    private void OnDisable()
    {
        _rewardButton.onClick.RemoveListener(OnButtonClick);
        _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        _noThanksButton.onClick.RemoveListener(InterAndSendEvent);
    }

    public void StartToWork()
    {
        if (_spiningRoulette == null)
            _spiningRoulette = StartCoroutine(SpiningRoulette());

        if (_hidingSkipButton == null)
            _hidingSkipButton = StartCoroutine(HidingSkipButton());
    }

    private void OnButtonClick()
    {
        _rewardButton.onClick.RemoveListener(OnButtonClick);
        _rewardButton.interactable = false;

        if (_spiningRoulette != null)
        {
            StopCoroutine(_spiningRoulette);
            _spiningRoulette = null;
            _slider.DOKill();
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            TryGetReward();
        }

    }

    private void OnSliderValueChanged(float value)
    {
        float newMultiplier = GetNewMultiplier(value);

        if (_currentMultiplier == newMultiplier)
        {
            return;
        }
        else
        {
            _currentMultiplier = newMultiplier;

            _currentMultiplierText.text = $"x{_currentMultiplier}";

            _reward = (int)(_wallet.MoneyEarnedPerLevel * _currentMultiplier);
            _rewardMoneyText.text = "+" + _reward.ToString();
        }
    }

    private float GetNewMultiplier(float sliderValue)
    {
        if (sliderValue >= 0 && sliderValue < 0.15f)
        {
            return 1.5f;
        }
        else if (sliderValue >= 0.15f && sliderValue < 0.365f)
        {
            return 2;
        }
        else if (sliderValue >= 0.365f && sliderValue <= 0.635f)
        {
            return 3;
        }
        else if (sliderValue > 0.635f && sliderValue <= 0.85f)
        {
            return 2;
        }
        else
        {
            return 1.5f;
        }

    }

    private IEnumerator SpiningRoulette()
    {
        _slider.value = 0f;

        yield return null;

        Tween tween;

        while (_spiningRoulette != null)
        {
            if (_slider.value < 1)
                tween = _slider.DOValue(1f, _oneCycleTime / 2);
            else
                tween = _slider.DOValue(0f, _oneCycleTime / 2);

            yield return tween.WaitForCompletion();
        }

        _spiningRoulette = null;
    }

    private IEnumerator HidingSkipButton()
    {
        var delay = new WaitForSeconds(2f);

        yield return delay;

        Tween tween = _noThanksButton.transform.DOScale(Vector3.one, 0.7f);

        yield return tween.WaitForCompletion();

        _hidingSkipButton = null;
    }

    private void TryGetReward()
    {
        EccentricInit.Instance.AdManager.ShowRewardAd(GetReward);
    }

    private void GetReward()
    {
        _wallet.Add(_reward);
        SendEndEvent();
    }

    private void InterAndSendEvent()
    {
        EccentricInit.Instance.AdManager.ShowAd();
        SendEndEvent();
    }

    private void SendEndEvent()
    {
        _rewardButton.onClick.RemoveListener(OnButtonClick);
        _noThanksButton.onClick.RemoveListener(SendEndEvent);

        if (_waitingBeforeSendEvent == null)
            _waitingBeforeSendEvent = StartCoroutine(WaitingBeforeSendEvent());
    }

    private IEnumerator WaitingBeforeSendEvent()
    {
        _rewardButton.interactable = false;

        yield return new WaitForSeconds(_delayBeforeSendEvent);

        RewardTakenOrSkipped?.Invoke();
        _waitingBeforeSendEvent = null;
    }
}
