using System;
using UnityEngine;

public class LevelEnder : MonoBehaviour
{
    [SerializeField] private Points _levelPoints;
    [SerializeField] private LevelCompleteScreen _completeScreen;
    [SerializeField] private CatFinisher _catFinisher;
    [SerializeField] private CanvasGroupView _walletUI;
    [SerializeField] private CanvasGroupView _gameUI;

    public Action LevelEnded;

    private void OnEnable()
    {
        _completeScreen.LevelEnded += OnLevelEnd;
        _completeScreen.TimeToSaveProgress += OnTimeToSaveProgress;
        _catFinisher.Finished += OnCatFinished;
    }

    private void OnDisable()
    {
        _completeScreen.LevelEnded -= OnLevelEnd;
        _completeScreen.TimeToSaveProgress -= OnTimeToSaveProgress;
        _catFinisher.Finished -= OnCatFinished;
    }

    private void OnTimeToSaveProgress()
    {
        GameProgressHolder.Instance.UpdateProgress(_levelPoints.Amount);
    }

    private void OnLevelEnd()
    {
        LevelLoader.Instance.LoadLevel();
    }

    private void OnCatFinished()
    {
        LevelEnded?.Invoke();

        _walletUI.SetVisibilityFast(true);
        _gameUI.SetVisibility(false);
        GameStopper.Instance.SetWorkingStatus(false);
    }
}