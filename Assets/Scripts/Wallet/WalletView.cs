using UnityEngine;
using TMPro;
using DG.Tweening;
using Eccentric;

public class WalletView : MonoBehaviour
{
    [SerializeField] private WalletViewUI _ui;
    [SerializeField] private float _amimationTime = 1f;

    private TMP_Text _text;
    private Wallet _wallet;
    private int _currentValue;

    private void Awake()
    {
        _wallet = GetComponent<Wallet>();
        _text = _ui.GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _wallet.AmountChanged += OnCoinsAmountChanged;
    }

    private void OnDisable()
    {
        _wallet.AmountChanged -= OnCoinsAmountChanged;
    }

    private void Start()
    {
        _currentValue = SaveSystemWithData.PlayerData.CurrencyAmount;
        _text.text = SaveSystemWithData.PlayerData.CurrencyAmount.ToString();
    }

    private void OnCoinsAmountChanged(int newAmount)
    {
        _text.DOCounter(_currentValue, newAmount, _amimationTime);
        _currentValue = newAmount;
        // _text.text = _reduction.GetReductedText(newAmount);
    }
}
