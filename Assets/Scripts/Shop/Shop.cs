using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Eccentric;
using TMPro;

public class Shop : MonoBehaviour
{
    private const string ChoosenSkinID = nameof(ChoosenSkinID);
    public static bool IsFetched;

    [SerializeField] private Wallet _wallet;
    [SerializeField] private ShopProduct _productPrefab;
    [SerializeField] private SkinBook _skins;
    [SerializeField] private CatSkinsController _catSkinsController;
    [Space]
    [Header("SkinName")]
    [SerializeField] private TMP_Text _skinName;
    [Space]
    [Header("ScrollView")]
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _contentPanel;
    [SerializeField] private RectTransform _productRectTransformSample;
    [SerializeField] private HorizontalLayoutGroup _layoutGroup;
    [SerializeField] private float _snappingSpeedStep = 100f;

    private List<ShopProduct> _products = new List<ShopProduct>();
    private int _currentSelectedSkinIndex = 0;
    private float _targetPositinX;
    private bool _isSnapped = false;
    private bool _isScaled = false;
    private float _oneProductSpace;
    private float _currentSnappingSpeed = 0;
    private bool _isInAppAvailable;
    private bool _isOpen = false;
    private bool _isCurrentSkinSelected = false;

    private void Awake()
    {
        SetInAppAvailability();

        _oneProductSpace = _productRectTransformSample.rect.width + _layoutGroup.spacing;

        for (int i = 0; i < _catSkinsController.Skins.Length; i++)
        {
            ShopProduct product = Instantiate(_productPrefab, _productPrefab.transform.parent);
            _products.Add(product);
            product.SetInAppAvailability(_isInAppAvailable);
            product.SkinData = _skins.GetSkinData(_catSkinsController.Skins[i].ID);
            product.gameObject.SetActive(true);

            foreach (var purchasedSkinID in SaveSystemWithData.PlayerData.SkinsID)
                if (purchasedSkinID == _products[i].SkinData.Id)
                    _products[i].SetPurchasedStatus();
        }

        if (GameEvent.Instance != null)
            GameEvent.Instance.SetShop(this);
    }

    private void OnEnable()
    {
        for (int i = 0; i < _products.Count; i++)
        {
            _products[i].ActivateButtonClick += OnActivateButtonClick;
            _products[i].BuyButtonClick += OnSkinPurchased;
        }

        _wallet.AmountChanged += OnWalletAmountChanged;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _products.Count; i++)
        {
            _products[i].ActivateButtonClick -= OnActivateButtonClick;
            _products[i].BuyButtonClick -= OnSkinPurchased;
        }

        _wallet.AmountChanged -= OnWalletAmountChanged;
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey(ChoosenSkinID))
            OnSkinActivate(_products[0]);

        for (int i = 0; i < _products.Count; i++)
        {
            if (PlayerPrefs.GetInt(ChoosenSkinID) == _products[i].SkinData.Id)
                OnSkinActivate(_products[i]);

            CheckAvailabilityToBuy(_products[i]);
        }

        if (!Shop.IsFetched)
        {
            EccentricInit.Instance.InAppPurchase.FetchPurchases();
            Shop.IsFetched = true;
        }
    }

    private void Update()
    {
        if (_isOpen)
        {
            if (!Input.GetMouseButton(0))
            {
                if (!_isCurrentSkinSelected)
                    _currentSelectedSkinIndex = Mathf.RoundToInt(0 - _contentPanel.localPosition.x / _oneProductSpace);

                if (_scrollRect.velocity.magnitude < 150 && !_isSnapped)
                {
                    _scrollRect.velocity = Vector2.zero;
                    _currentSnappingSpeed += _snappingSpeedStep * Time.deltaTime;

                    _targetPositinX = 0 - (_currentSelectedSkinIndex * _oneProductSpace);

                    _contentPanel.localPosition = new Vector3(
                      Mathf.MoveTowards(_contentPanel.localPosition.x, _targetPositinX, _currentSnappingSpeed),
                        _contentPanel.localPosition.y,
                        _contentPanel.localPosition.z);

                    if (_contentPanel.localPosition.x == _targetPositinX)
                    {
                        _products[_currentSelectedSkinIndex].transform.DOScale(1.35f, 0.7f);
                        OnSkinActivate(_products[_currentSelectedSkinIndex]);
                        _isSnapped = true;
                        _isCurrentSkinSelected = false;
                        _currentSnappingSpeed = 0;
                    }
                }
            }
            else
            {
                if (_scrollRect.velocity.magnitude > 1 && _isSnapped)
                {
                    _isSnapped = false;
                    _isScaled = false;
                }
            }

            if (!_isScaled && !_isSnapped)
            {
                for (int i = 0; i < _products.Count; i++)
                {
                    _products[i].transform.DOKill();
                    _products[i].transform.DOScale(1f, 0.7f);
                }


                _isScaled = true;
            }
        }
    }

    public void SetShopStatus(bool isOpen)
    {
        _isOpen = isOpen;
    }

    public void OnShopClosed()
    {
        foreach (var productHolder in _products)
        {
            if (PlayerPrefs.GetInt(ChoosenSkinID) == productHolder.SkinData.Id)
            {
                OnSkinActivate(productHolder);
            }
        }
    }

    private void OnSkinPurchased(ShopProduct product)
    {
        product.BuyButtonClick -= OnSkinPurchased;

        if (!product.SkinData.IsInApp)
            _wallet.Spend(product.SkinData.Cost);

        product.SetPurchasedStatus();

        SaveSystemWithData.PlayerData.SkinsID.Add(product.SkinData.Id);
        EccentricInit.Instance.SaveSystemWithData.Save(SaveSystemWithData.PlayerData, true);

        if (_products[_currentSelectedSkinIndex] == product)
            OnSkinActivate(product);
    }

    private void OnActivateButtonClick(ShopProduct product)
    {
        _isCurrentSkinSelected = true;
        _isSnapped = false;
        _isScaled = false;
        _currentSelectedSkinIndex = _products.IndexOf(product);
    }

    private void OnSkinActivate(ShopProduct product)
    {
        _catSkinsController.ActivateSkin(product.SkinData.Id.ToString());
        _skinName.text = EccentricInit.Instance.LocalisationManager.GetText(product.SkinData.NameID);

        if (product.IsPurchased)
        {
            PlayerPrefs.SetInt(ChoosenSkinID, product.SkinData.Id);
        }

        foreach (var productHolder in _products)
        {
            if (productHolder.IsPurchased)
            {
                productHolder.SetChoosenStatus(false);
            }
        }

        product.SetChoosenStatus(true);
    }

    private void OnWalletAmountChanged(int softCurrencyAvailable)
    {
        for (int i = 0; i < _products.Count; i++)
        {
            CheckAvailabilityToBuy(_products[i]);
        }
    }

    private void CheckAvailabilityToBuy(ShopProduct product)
    {
        if (!product.IsPurchased)
        {
            if (product.SkinData.IsInApp)
            {
                product.SetAvailabilityToBuy(true);
            }
            else
            {
                if (_wallet.IsAbleToSpend(product.SkinData.Cost))
                    product.SetAvailabilityToBuy(true);
                else
                    product.SetAvailabilityToBuy(false);
            }
        }
    }

    private void SetInAppAvailability()
    {
        var currentPlatform = EccentricInit.Instance.Platform;

        switch (currentPlatform)
        {
            case Platform.YANDEX:
                _isInAppAvailable = true;
                break;
            case Platform.OK:
                _isInAppAvailable = true;
                break;
            case Platform.VK:
                _isInAppAvailable = true;
                break;
            default:
                _isInAppAvailable = false;
                break;
        }
    }

    public void ForceActivateSkin(int id)
    {
        for (int i = 0; i < _products.Count; i++)
        {
            if (_products[i].SkinData.Id == id)
            {
                _products[i].SetPurchasedStatus();

                _catSkinsController.ActivateSkin(_products[i].SkinData.Id.ToString());
                PlayerPrefs.SetInt(ChoosenSkinID, _products[i].SkinData.Id);

                foreach (var productHolder in _products)
                {
                    if (productHolder.IsPurchased)
                    {
                        productHolder.SetChoosenStatus(false);
                    }
                }

                _products[i].SetChoosenStatus(true);
            }
        }
    }
}
