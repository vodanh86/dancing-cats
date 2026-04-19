using System;
using TMPro;
using UnityEngine;

public class PlayerGameStoper : MonoBehaviour
{
    [SerializeField] private CanvasGroupView _wingsUI;
    [SerializeField] private CanvasGroupView _loseUI;
    [SerializeField] private TMP_Text _livesCounterUI;
    [SerializeField] private TMP_Text _livesCounterUISecond;

    private PlayerMovement _movement;
    private int _playerLives = 2;
    private int _livesPerADS = 1;
    private Action _callback;

    public int PlayerLives => _playerLives;
    public bool IsHaveLives => _playerLives > 1;

    public Action LevelFailed;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    public void StartGame()
    {
        SongManager.Instance.StartMusic();
        _movement.CanMove = true;
        //  _livesCounterUI.text = _playerLives.ToString();
        _livesCounterUISecond.text = _playerLives.ToString();
    }

    public void StopGame()
    {
        SongManager.Instance.StopMusic();
        _movement.CanMove = false;

        _wingsUI.SetVisibilityFast(false);
        _loseUI.SetVisibility(true);
        LevelFailed?.Invoke();
    }

    public void PauseGame()
    {
        SongManager.Instance.PauseMusic();
        _movement.CanMove = false;

        UpdateWingView();
        _wingsUI.SetVisibility(true);
    }

    public void ContinueGame(Action callback)
    {
        _playerLives += _livesPerADS;

        //  _livesCounterUI.text = _playerLives.ToString();
        _livesCounterUISecond.text = _playerLives.ToString();

        _wingsUI.SetVisibilityFast(false);

        if (callback != null)
            _callback = callback;

        GameStopper.Instance.StartCountingDown(() =>
        {
            SongManager.Instance.UnpauseMusic();
            _callback.Invoke();
            _movement.CanMove = true;
        });
    }

    public void UpdateWingView()
    {
        _playerLives--;
        // _livesCounterUI.text = _playerLives.ToString();
        _livesCounterUISecond.text = _playerLives.ToString();
    }
}
