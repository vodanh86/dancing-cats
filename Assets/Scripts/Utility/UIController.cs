using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    //[SerializeField] private CanvasGroup[] _startGroup;
    //[SerializeField] private CanvasGroup[] _playingGroup;
    //[SerializeField] private CanvasGroup _tutorial;

    //private Coroutine _waitingToStartPlay;
    //private float _tutorialFadeSpeed = 2f;
    //private float _timeLeftTutorialBeHide;
    //private const float TutorialHideTime = 2f;

    //public event Action PlayingStarted;

    //private void Awake()
    //{
    //    foreach (var group in _playingGroup)
    //    {
    //        group.alpha = 0;
    //        group.interactable = false;
    //        group.blocksRaycasts = false;
    //    }
    //}

    //private void Start()
    //{
    //    if (_waitingToStartPlay == null)
    //        _waitingToStartPlay = StartCoroutine(WaitingToStartPlay());
    //}

    //private IEnumerator WaitingToStartPlay()
    //{

    //    yield return null;

    //    PlayingStarted?.Invoke();

    //    foreach (var group in _startGroup)
    //        SwitchGroupStatus(group, false);

    //    foreach (var group in _playingGroup)
    //        SwitchGroupStatus(group, true);

    //    _waitingToStartPlay = null;
    //}

    //private void SwitchGroupStatus(CanvasGroup group, bool isActive)
    //{
    //    group.DOFade(isActive ? 1f : 0, 0.5f);
    //    group.interactable = isActive;
    //    group.blocksRaycasts = isActive;
    //}
}
