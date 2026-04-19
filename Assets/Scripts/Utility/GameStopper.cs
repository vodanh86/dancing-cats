using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStopper : MonoBehaviour
{
    public static GameStopper Instance;

    [SerializeField] private CanvasGroupView _view;
    [SerializeField] private TMP_Text _text;

    private bool _isGameStoped = false;
    private bool _isPlayingSong = false;
    private Coroutine _countingDown;

    public bool IsGameStoped => _isGameStoped;
    public bool IsPlayingSong => _isPlayingSong;
    public bool IsCounting => _countingDown != null;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        _text.text = "3";
        _view.SetVisibilityFast(false);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            Debug.Log("focus");
        else
            Debug.Log("unFocus");
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            Debug.Log("pause");
        else
            Debug.Log("unPause");
    }

    public void SetStopGameStatus()
    {
        _isGameStoped = true;
    }

    public void SetWorkingStatus(bool isWorking)
    {
        _isPlayingSong = isWorking;
    }

    public void StartCountingDown(Action callback)
    {
        if (!_isPlayingSong)
            return;

        if (_countingDown == null)
            _countingDown = StartCoroutine(CountingDown(callback));
    }

    public void RestartCountingDown(Action callback)
    {
        if (_countingDown != null)
        {
            StopCoroutine(_countingDown);
            _countingDown = null;
        }

        _countingDown = StartCoroutine(CountingDown(callback));
    }

    public void StopCountingDown()
    {
        if (_countingDown != null)
        {
            StopCoroutine(_countingDown);
            _countingDown = null;
        }
    }

    private IEnumerator CountingDown(Action callback)
    {
        if (!_isGameStoped)
            _isGameStoped = true;

        var delay = new WaitForSecondsRealtime(1f);

        _view.SetVisibilityFast(true);

        _text.text = "3";

        yield return delay;

        _text.text = "2";

        yield return delay;

        _text.text = "1";

        yield return delay;

        _view.SetVisibilityFast(false);

        callback.Invoke();

        _isGameStoped = false;
        _countingDown = null;
    }
}
