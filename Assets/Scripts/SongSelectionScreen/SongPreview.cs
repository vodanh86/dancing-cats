using Eccentric;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongPreview : MonoBehaviour
{
    [Header("SetupImages")]
    [SerializeField] private Image _songIcon;
    [SerializeField] private Image _playButtonIcon;
    [SerializeField] private Image _playButtonBackgorund;
    [SerializeField] private Image _pointsLockIcon;
    [Space]
    [Header("CurrencyIcons")]
    [SerializeField] private Sprite _lockedSongSprite;
    [SerializeField] private Sprite _unlockedSongSprite;
    [Space]
    [Header("ButtonBackgrounds")]
    [SerializeField] private Sprite _lockedBackground;
    [SerializeField] private Sprite _unlockedBackground;
    [Space]
    [Header("Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _unlockButton;
    [Space]
    [Header("Textes")]
    [SerializeField] private TMP_Text _songName;
    [SerializeField] private TMP_Text _earnedPoints;
    [Space]
    [Header("Pattern")]
    [SerializeField] private Mask _pattern;

    private SceneData _sceneData;
    private bool _isPurchased = false;

    public bool IsPurchased => _isPurchased;

    public Action<SceneData> PlayButtonClick;

    public SceneData SceneData
    {
        get
        {
            return _sceneData;
        }
        set
        {
            _sceneData = value;

            _songName.text = value.Name;
            _songIcon.sprite = value.SongIcon;

            _playButton.interactable = false;
            _playButtonBackgorund.sprite = _lockedBackground;
            _playButtonIcon.sprite = _lockedSongSprite;

            _earnedPoints.text = "xxx";
            _earnedPoints.gameObject.SetActive(false);
            _pointsLockIcon.gameObject.SetActive(true);

            if (_sceneData.IsEventSong)
                _pattern.gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnPlayButtonClick);
        _unlockButton.onClick.AddListener(TryToUnlock);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClick);
        _unlockButton.onClick.RemoveListener(TryToUnlock);
    }

    private void OnPlayButtonClick()
    {
        PlayButtonClick?.Invoke(this.SceneData);
    }

    private void TryToUnlock()
    {
        EccentricInit.Instance.AdManager.ShowRewardAd(() =>
        {
            GameProgressHolder.Instance.OpenLevel(_sceneData.Id);
            SetUnlockedStatus(0);
        });
    }

    public void SetUnlockedStatus(int earnedPoints)
    {
        _unlockButton.interactable = false;
        _unlockButton.gameObject.SetActive(false);

        _playButton.interactable = true;
        _playButtonBackgorund.sprite = _unlockedBackground;
        _playButtonIcon.sprite = _unlockedSongSprite;

        _earnedPoints.text = earnedPoints.ToString();
        _earnedPoints.gameObject.SetActive(true);
        _pointsLockIcon.gameObject.SetActive(false);
    }
}
