using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class PointsView : MonoBehaviour
{
    [SerializeField] private PointsViewUI _ui;
    [SerializeField] private float _amimationTime = 0.7f;
    [SerializeField] private TMP_Text _finalScreenText;
    [SerializeField] private Image _newRecord;

    private TMP_Text _text;
    private Points _points;
    private int _currentValue;

    private void Awake()
    {
        _points = GetComponent<Points>();
        _text = _ui.GetComponent<TMP_Text>();
        _newRecord.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _points.AmountChanged += OnPointsAmountChanged;
    }

    private void OnDisable()
    {
        _points.AmountChanged -= OnPointsAmountChanged;
    }

    private void Start()
    {
        _currentValue = _points.Amount;
        _text.text = _points.Amount.ToString();
    }

    private void OnPointsAmountChanged(int newAmount)
    {
        _text.DOCounter(_currentValue, newAmount, _amimationTime);
        _currentValue = newAmount;
        _finalScreenText.text = newAmount.ToString();
    }

    public void CheckForNewRecord()
    {
        if (!_newRecord.gameObject.activeSelf)
            if (GameProgressHolder.Instance.GetCurrentSceneScore() < _currentValue)
                _newRecord.gameObject.SetActive(true);
    }
}
