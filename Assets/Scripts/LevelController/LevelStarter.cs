using UnityEngine;
using TMPro;
using Eccentric;
using System.Collections;
using System;

public class LevelStarter : MonoBehaviour
{
    [SerializeField] private PlayerPhaseSwitcher _phaseSwitcher;
    [SerializeField] private StartButtonUI _startButtonUI;
    [SerializeField] private CanvasGroupView _mainUI;
    [SerializeField] private CanvasGroupView _gameUI;
    [SerializeField] private CanvasGroupView _walletUI;
    [SerializeField] private TMP_Text _levelName;
    [SerializeField] private TMP_Text _songName;
    [SerializeField] private SongSelectionScreen _songSelection;

    private Coroutine _waitingToPressSpace;
    public Action LevelStarted;


    private void Awake()
    {
        _gameUI.SetVisibilityFast(false);
        _mainUI.SetVisibilityFast(true);
        GameProgressHolder.Instance.UpdateCurrentSceneData();
    }

    private void OnEnable()
    {
        _startButtonUI.GameStarted += OnButtonClick;
    }

    private void OnDisable()
    {
        _startButtonUI.GameStarted -= OnButtonClick;
    }

    private void Start()
    {
        GameProgressHolder.Instance.OpenLevel();
        _songSelection.СheckSongForAvailability();
        _levelName.text = GameProgressHolder.Instance.CurrentSceneData.Name;
        _songName.text = GameProgressHolder.Instance.CurrentSceneData.Name;

        //Invoke(nameof(ShowADS), 1f);

        if (SaveSystemWithData.PlayerData.CurrentLevel >= 3)
        {
            if (PlayerPrefs.HasKey("IsShortcut"))
            {
                PlayerPrefs.SetInt("IsShortcut", 1);
                EccentricInit.Instance.App.ShowRequestShortcut();
            }
        }

        if (SaveSystemWithData.PlayerData.CurrentLevel >= 5)
        {
            if (PlayerPrefs.HasKey("IsReview"))
            {
                PlayerPrefs.SetInt("IsReview", 1);
                EccentricInit.Instance.App.ShowRequestReview();
            }
        }

        //_waitingToPressSpace = StartCoroutine(WaitingToPressSpace());
    }

    private void ShowADS()
    {
        // EccentricInit.Instance.AdManager.ShowAd();
    }

    public void OnButtonClick()
    {
        _startButtonUI.GameStarted -= OnButtonClick;
        _startButtonUI.gameObject.SetActive(false);
        LevelLoader.Instance.SetOverlayVisible(false);
        _phaseSwitcher.StartSearchingBlock();

        GameProgressHolder.Instance.SendLevelStartedEvent();

        LevelStarted?.Invoke();

        _gameUI.SetVisibilityFast(true);
        _mainUI.gameObject.SetActive(false);
        _walletUI.SetVisibility(false);
        GameStopper.Instance.SetWorkingStatus(true);

        // StopCoroutine(_waitingToPressSpace);
        // _waitingToPressSpace = null;
    }

    //private IEnumerator WaitingToPressSpace()
    //{
    //    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
    //    OnButtonClick();
    //}
}
