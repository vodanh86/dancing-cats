using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class LevelLoaderView : MonoBehaviour
{
    [SerializeField] private Slider _bar;
    [SerializeField] private TMP_Text _percent;
    [SerializeField] private CanvasGroup _canvasGroup;

    private const float DonePercentDivider = 0.9f;
    private Coroutine _loadingCoroutine;
    private AsyncOperation _asyncOperation;

    public void ShowLoadingProgress(AsyncOperation asyncOperation)
    {
        _asyncOperation = asyncOperation;

        if (_loadingCoroutine == null)
        {
            _loadingCoroutine = StartCoroutine(LoadingScene());
        }
    }

    private IEnumerator LoadingScene()
    {
        float speed = 1f;

        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        while (_asyncOperation.progress < DonePercentDivider)
        {
            float progress = _asyncOperation.progress * 100;

            _bar.value = Mathf.MoveTowards(_bar.value, progress, speed * Time.deltaTime);
            _percent.text = ((int)(_bar.value)).ToString();
            yield return null;
        }

        _percent.DOCounter((int)_bar.value, (int)_bar.maxValue - 1, 1f);
        _bar.DOValue(_bar.maxValue - 1, 1f);

        while (SongAudioSource.Instance.IsLoading)
            yield return null;

        _percent.DOKill();
        _bar.DOKill();

        yield return null;

        Tween tween;
        _percent.DOCounter((int)_bar.value, (int)_bar.maxValue, 1f);
        tween = _bar.DOValue(_bar.maxValue, 1f);

        yield return tween.WaitForCompletion();

        yield return new WaitForSeconds(1f);

        tween = _canvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
        {
            LevelLoader.Instance.SendGameReady();

            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        });

        yield return tween.WaitForCompletion();


        _bar.value = 0;
        _percent.text = "0";

        _loadingCoroutine = null;
    }
}

