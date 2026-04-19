using Eccentric;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    private const string GameLogic = nameof(GameLogic);
    private SceneData _sceneToLoad;
    private const float DonePercentDivider = 0.9f;
    private Coroutine _loadingCoroutine;
    private LevelLoaderView _view;
    private bool _isFirstTime = true;

    public Action LevelLoaded;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        _view = GetComponent<LevelLoaderView>();
    }

    public void RestartLevel()
    {
        _sceneToLoad = GameProgressHolder.Instance.CurrentSceneData;

        if (_loadingCoroutine == null)
            _loadingCoroutine = StartCoroutine(LoadingScene());
    }

    public void LoadLevel()
    {
        _sceneToLoad = GameProgressHolder.Instance.GetNextSceneData();

        if (_loadingCoroutine == null)
            _loadingCoroutine = StartCoroutine(LoadingScene());
    }

    public void LoadLevel(SceneData sceneToLoad)
    {
        _sceneToLoad = sceneToLoad;

        if (_loadingCoroutine == null)
            _loadingCoroutine = StartCoroutine(LoadingScene());
    }

    private IEnumerator LoadingScene()
    {
        SongAudioSource.Instance.LoadClip(_sceneToLoad.SongKey.ToString());

        yield return new WaitForEndOfFrame();

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneToLoad.Name);
        asyncOperation.allowSceneActivation = false;

        _view.ShowLoadingProgress(asyncOperation);

        SceneManager.LoadScene(GameLogic, LoadSceneMode.Additive);

        while (asyncOperation.progress < DonePercentDivider)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        asyncOperation.allowSceneActivation = true;

        LevelLoaded?.Invoke();

        _loadingCoroutine = null;
    }

    public void SendGameReady()
    {
        if (_isFirstTime)
        {
            EccentricInit.Instance.App.GameReady();
            _isFirstTime = false;
        }
    }
}
