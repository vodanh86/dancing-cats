using Eccentric;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    private const string GameLogic = nameof(GameLogic);
    [SerializeField] private GameObject _overlayRoot;
    [SerializeField] private Camera _overlayCamera;
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

        if (_overlayRoot == null)
        {
            var canvas = GetComponentInChildren<Canvas>(true);
            if (canvas != null)
                _overlayRoot = canvas.gameObject;
        }

        if (_overlayCamera == null)
            _overlayCamera = GetComponentInChildren<Camera>(true);
    }

    public void RestartLevel()
    {
        _sceneToLoad = GameProgressHolder.Instance.CurrentSceneData;

        StartLoadingSceneIfNeeded();
    }

    public void LoadLevel()
    {
        _sceneToLoad = GameProgressHolder.Instance.GetNextSceneData();

        StartLoadingSceneIfNeeded();
    }

    public void LoadLevel(SceneData sceneToLoad)
    {
        _sceneToLoad = sceneToLoad;

        StartLoadingSceneIfNeeded();
    }

    private void StartLoadingSceneIfNeeded()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        if (!enabled)
            enabled = true;

        SetOverlayVisible(true);

        if (_loadingCoroutine == null)
            _loadingCoroutine = StartCoroutine(LoadingScene());
    }

    public void SetOverlayVisible(bool isVisible)
    {
        if (_overlayRoot == null)
        {
            var canvas = GetComponentInChildren<Canvas>(true);
            if (canvas != null)
                _overlayRoot = canvas.gameObject;
        }

        if (_overlayCamera == null)
            _overlayCamera = GetComponentInChildren<Camera>(true);

        if (_overlayRoot != null)
            _overlayRoot.SetActive(isVisible);

        if (_overlayCamera != null)
            _overlayCamera.gameObject.SetActive(isVisible);
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
