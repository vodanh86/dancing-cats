using UnityEngine;
using DG.Tweening;
using System;
using AssetKits.ParticleImage;
using System.Collections;

public class LevelCompleteScreen : MonoBehaviour
{
    [Header("ScreenCanvasGroup")]
    [SerializeField] private CanvasGroup _levelCompleteScreen;
    [Space]
    [Header("Particle")]
    [SerializeField] private ParticleImage _particleImage;
    [Space]
    [Header("Score")]
    [SerializeField] private PointsView _pointsView;
    [SerializeField] private LevelProgressBarStar[] _stars;
    [SerializeField] private Color _starsColor;
    [Space]
    [Header("FinalRouletteRewarder")]
    [SerializeField] private RouletteRewarder _rouletteRewarder;

    private Finisher _finisher;
    private float _showingTime = 0.5f;
    private Coroutine _playingScoreAnimation;

    public event Action LevelEnded;
    public event Action TimeToSaveProgress;

    private void Awake()
    {
        _levelCompleteScreen.alpha = 0f;
        _levelCompleteScreen.interactable = false;
        _levelCompleteScreen.blocksRaycasts = false;

    }

    private void OnEnable()
    {
        _finisher = Finisher.Instance;
        _finisher.LevelEnded += ShowScreen;
        _rouletteRewarder.RewardTakenOrSkipped += SendLevelEndEvent;
    }

    private void OnDisable()
    {
        _finisher.LevelEnded -= ShowScreen;
        _rouletteRewarder.RewardTakenOrSkipped -= SendLevelEndEvent;
    }

    private void ShowScreen()
    {
        _rouletteRewarder.StartToWork();
        _levelCompleteScreen.DOFade(1f, _showingTime).OnComplete(() =>
        {
            TimeToSaveProgress?.Invoke();
            _levelCompleteScreen.interactable = true;
            _levelCompleteScreen.blocksRaycasts = true;

            if (_playingScoreAnimation == null)
                _playingScoreAnimation = StartCoroutine(PlayingScoreAnimation());
        });
    }

    private void SendLevelEndEvent()
    {
        LevelEnded?.Invoke();
    }

    private IEnumerator PlayingScoreAnimation()
    {
        var delay = new WaitForSeconds(0.5f);

        for (int i = 0; i < _stars.Length; i++)
        {
            yield return delay;

            _stars[i].Activate();
        }

        _pointsView.CheckForNewRecord();

        yield return delay;

        _particleImage.Play();

    }
}
