using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GamePush;
using Eccentric;
using System.Collections;

public class ShopProduct : MonoBehaviour
{
    [Header("SetupImages")]
    [SerializeField] private Image _background;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _buyButtonImage;
    [Space]
    [Header("Buttons")]
    [SerializeField] private Button _activateButton;
    [SerializeField] private Button _buyButton;
    [Space]
    [Header("CostText")]
    [SerializeField] private TMP_Text _cost;
    [SerializeField] private TMP_Text _eventCost;
    [Space]
    [Header("Colors")]
    [SerializeField] private Color _softCurrencyColor;
    [SerializeField] private Color _notEnoughCurrencyColor;
    [SerializeField] private Color _inAppCurrencyColor;
    [SerializeField] private Color _rewardCurrencyColor;
    [SerializeField] private Color _eventCurrencyColor;
    [Space]
    [Header("CurrencyIcons")]
    [SerializeField] private Image _currencyIcon;
    [SerializeField] private Image _inAppCurrencyIcon;
    [SerializeField] private Image _rewardIcon;

    private bool _isInAppAvailable;
    private int _adsWatched = 0;
    private SkinData _skinData;
    private bool _isPurchased = false;

    public bool IsPurchased => _isPurchased;
    public Button BuyButton => _buyButton;
    public Button ActivateButton => _activateButton;

    public Action<ShopProduct> ActivateButtonClick;
    public Action<ShopProduct> BuyButtonClick;

    public SkinData SkinData
    {
        get
        {
            return _skinData;
        }
        set
        {
            _skinData = value;
            _icon.sprite = value.Icon;
            _buyButton.interactable = false;

            if (value.IsInApp)
            {
                _currencyIcon.gameObject.SetActive(false);

                if (value.IsForReward)
                {
                    if (SaveSystemWithData.PlayerData.IsSkinInList(value.Id))
                        _adsWatched = SaveSystemWithData.PlayerData.GetSkinADWatched(value.Id);
                    else
                        _adsWatched = 0;

                    _cost.text = _adsWatched.ToString() + '/' + value.AddsWatchedToUnlock.ToString() + " <sprite=\"ad\" index=0>";
                    _rewardIcon.gameObject.SetActive(false);
                    _buyButtonImage.color = _rewardCurrencyColor;
                }
                else
                {
                    _inAppCurrencyIcon.gameObject.SetActive(false);
                    _buyButtonImage.color = _inAppCurrencyColor;

                    if (EccentricInit.Instance.InAppPurchase.FetchProductsList.Count != 0)
                        _cost.text = EccentricInit.Instance.InAppPurchase.GetPrice(_skinData.Id, true);
                }
            }
            else
            {
                if (value.IsEventSkin)
                {
                    _cost.gameObject.SetActive(false);
                    _eventCost.gameObject.SetActive(true);
                    _eventCost.text = EccentricInit.Instance.LocalisationManager.GetText("EvenSkin");
                    _currencyIcon.gameObject.SetActive(false);
                    _inAppCurrencyIcon.gameObject.SetActive(false);
                    _buyButtonImage.color = _eventCurrencyColor;
                    _buyButton.interactable = true;
                    _buyButton.enabled = false;
                }
                else
                {
                    _inAppCurrencyIcon.gameObject.SetActive(false);
                    _currencyIcon.gameObject.SetActive(false);
                    _buyButtonImage.color = _notEnoughCurrencyColor;
                    _cost.text = value.Cost.ToString() + " <sprite=\"coin\" index=0>";
                }
            }
        }
    }

    private IEnumerator Start()
    {
        if (!_skinData.IsInApp && !_skinData.IsForReward) yield break;

        yield return new WaitUntil(() => EccentricInit.Instance.InAppPurchase.FetchProductsList.Count != 0);

        _cost.text = EccentricInit.Instance.InAppPurchase.GetPrice(_skinData.Id, true);
    }

    private void OnEnable()
    {
        EccentricInit.Instance.InAppPurchase.OnPurchaseEvent += GivePurchase;
        EccentricInit.Instance.InAppPurchase.OnFetchEvent += GivePurchase;

        _activateButton.onClick.AddListener(OnActivateButtonClick);
        _buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    private void OnDisable()
    {
        EccentricInit.Instance.InAppPurchase.OnPurchaseEvent -= GivePurchase;
        EccentricInit.Instance.InAppPurchase.OnFetchEvent -= GivePurchase;

        _activateButton.onClick.RemoveListener(OnActivateButtonClick);
        _buyButton.onClick.RemoveListener(OnBuyButtonClick);
    }

    public void SetPurchasedStatus()
    {
        _isPurchased = true;

        _buyButton.gameObject.SetActive(false);
    }

    public void SetAvailabilityToBuy(bool isAvailable)
    {
        if (SkinData.IsEventSkin)
            return;

        _buyButton.interactable = isAvailable;

        if (!SkinData.IsInApp)
            _buyButtonImage.color = isAvailable ? _softCurrencyColor : _notEnoughCurrencyColor;
    }

    public void SetChoosenStatus(bool isChoosen)
    {
    }

    private void OnActivateButtonClick()
    {
        ActivateButtonClick?.Invoke(this);
    }

    private void OnBuyButtonClick()
    {
        if (SkinData.IsInApp)
        {
            if (_isInAppAvailable)
            {
                if (_skinData.IsForReward)
                    TryWatchRewardAd();
                else
                    TryToBuyInAppSkin();
            }
            else
            {
                TryWatchRewardAd();
            }
        }
        else
        {
            if (!SkinData.IsEventSkin)
                Buy();
        }
    }

    private void TryToBuyInAppSkin()
    {
        EccentricInit.Instance.InAppPurchase.Purchase(_skinData.Id.ToString());
    }

    private void TryWatchRewardAd()
    {
        EccentricInit.Instance.AdManager.ShowRewardAd(GetReward);
    }

    private void GetReward()
    {
        _adsWatched++;
        SaveSystemWithData.PlayerData.SetSkinADWatched(new RewardSkins(SkinData.Id, _adsWatched));

        if (_adsWatched >= SkinData.AddsWatchedToUnlock)
        {
            Buy();
        }
        else
        {
            EccentricInit.Instance.SaveSystemWithData.Save(SaveSystemWithData.PlayerData);
            _cost.text = _adsWatched.ToString() + '/' + SkinData.AddsWatchedToUnlock.ToString() + " <sprite=\"ad\" index=0>";
        }
    }

    private void GivePurchase(string id)
    {
        if (id == _skinData.Id.ToString())
        {
            if (_skinData.IsInApp)
                CongratulationsPurchase.Instance.Activate(_skinData.Icon);

            Buy();
        }
    }

    private void Buy()
    {
        BuyButtonClick?.Invoke(this);
    }

    public void SetInAppAvailability(bool isAvailable)
    {
        _isInAppAvailable = isAvailable;
    }
}
