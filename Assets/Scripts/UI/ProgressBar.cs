using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _currentLocationIcon;
    [SerializeField] private Image _nextlocationIcon;
    [SerializeField] private Image[] _currentLocationSteps;
    [SerializeField] private Image[] _currentLocationWaySteps;
    [SerializeField] private ScenesBook _scenesBook;

    private GameProgress _progress = new GameProgress();
    private const int _levelsInOneGrop = 5;

    private int _currentBookGoupIndex;
    private int _currentLevelIndex;

    //private void Awake()
    //{
    //    _progress.Load();

    //    for (int i = 0; i < _currentLocationSteps.Length; i++)
    //    {
    //        _currentLocationSteps[i].gameObject.SetActive(false);
    //        _currentLocationWaySteps[i].gameObject.SetActive(false);
    //    }
    //}

    //public void Start()
    //{
    //    int currentGroupIndex = _progress.CurrentLevel / _levelsInOneGrop;
    //    _currentBookGoupIndex = currentGroupIndex % _scenesBook.ScenesGroups.Length;
    //    _currentLevelIndex = ((_progress.CurrentLevel - 1) % _levelsInOneGrop) + 1;

    //    SetLocationIcons();
    //    SetLevelIndexIcons();
    //}

    //private void SetLocationIcons()
    //{
    //    _currentLocationIcon.sprite = _scenesBook.ScenesGroups[_currentBookGoupIndex].GroupIcon;

    //    int nextBookGoupIndex;

    //    if (_currentBookGoupIndex + 1 >= _scenesBook.ScenesGroups.Length)
    //        nextBookGoupIndex = 0;
    //    else
    //        nextBookGoupIndex = _currentBookGoupIndex + 1;

    //    _nextlocationIcon.sprite = _scenesBook.ScenesGroups[nextBookGoupIndex].GroupIcon;
    //}

    //private void SetLevelIndexIcons()
    //{
    //    for (int i = 0; i < _currentLevelIndex; i++)
    //    {
    //        _currentLocationSteps[i].gameObject.SetActive(true);
    //        _currentLocationWaySteps[i].gameObject.SetActive(true);
    //    }
    //}
}
