using System.Collections;
using UnityEngine;

public class GameEventTimeCounter : MonoBehaviour
{
    [SerializeField] private LevelStarter _levelStarter;
    [SerializeField] private LevelEnder _levelEnder;
    [SerializeField] private PlayerGameStoper _playerGameStoper;

    private float _time;
    private Coroutine _counting;
    private bool _isEventAvailable;

    private void OnEnable()
    {
        _levelStarter.LevelStarted += StartCount;
        _levelEnder.LevelEnded += EndCount;
        _playerGameStoper.LevelFailed += EndCount;
    }

    private void OnDisable()
    {
        _levelStarter.LevelStarted -= StartCount;
        _levelEnder.LevelEnded -= EndCount;
        _playerGameStoper.LevelFailed -= EndCount;
    }

    private void Start()
    {
        _time = 0;

        if (GameEvent.Instance != null)
            _isEventAvailable = !GameEvent.Instance.GameEventData.GotSkin;
    }

    private void StartCount()
    {
        if (_isEventAvailable)
            if (_counting == null)
                _counting = StartCoroutine(Counting());
    }

    private void EndCount()
    {
        if (_counting != null)
        {
            GameEvent.Instance.AddPlayedTime((int)_time);
            StopCoroutine(_counting);
            _counting = null;
        }
    }

    private IEnumerator Counting()
    {
        while (_isEventAvailable)
        {
            _time += Time.deltaTime;
            yield return null;
        }

        _counting = null;
    }
}
