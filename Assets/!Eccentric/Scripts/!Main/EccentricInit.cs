using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if GAMEDISTRIBUTION
#elif LAGGED
#elif JIO
using com.jiogames.wrapper;
#else
using GamePush;
#endif

namespace Eccentric
{
    public class EccentricInit : MonoBehaviour
    {
        public static EccentricInit Instance { get; private set; }

        [field: SerializeField]
        public Language Language { get; private set; }


        [field: SerializeField] public Platform Platform { get; private set; }

        [field: SerializeField] public bool IsMobile { get; private set; }
        public SaveSystemWithData SaveSystemWithData { get; private set; }
        public SaveSystemPrefs SaveSystemPrefs { get; private set; }
        public AdManager AdManager { get; private set; }
        public InAppPurchase InAppPurchase { get; private set; }
        public LocalisationManager LocalisationManager { get; private set; }
        public LeaderboardManager LeaderboardManager { get; private set; }
        public AnalyticManager AnalyticManager { get; private set; }
        public App App { get; private set; }
        public PromoGame PromoGame { get; private set; }

        public CollectionPanelNew CollectionPanelNew { get; private set; }

        private SaveSystem _saveSystem;
        private SetQualityGraphic _setQualityGraphic;
        private List<LocalisationDataSO> _localisationDatas = new();

        [Space(30), SerializeField]
        private SaveType _saveType = SaveType.LocalAndCloud;

        public SaveType SaveType => _saveType;

        [SerializeField] private SaveSystemType _saveSystemType = SaveSystemType.Data;

        [Space(30), SerializeField]
        private bool _isEnableTextSwitcher;

        [SerializeField] private bool _isEnableConsoleLog;
        private TextSwitcher _textSwitcher;
        private ConsoleViewer _consoleViewer;


        [Space(30)] public Publisher Publisher = Publisher.Eccentric;

        [SerializeField]
        private InAppIdType _inAppIdType;

        public static InAppIdType InAppIdType { get; private set; }


        public BuildType BuildType;

        [SerializeField]
        private string _token;

        private bool _isTokenValid;

        [SerializeField]
        private string _gameDistributionKey;

        [SerializeField]
        private string _developerId;

        [SerializeField]
        private string _publisherId;

        [SerializeField]
        private string _adSpotInterstitial;

        [SerializeField]
        private string _adSpotRewardedVideo;

        [SerializeField]
        private string _packageName;

        private readonly float _timeBeforeInitialize = 1f;
        public static bool IsInitialized;
        private const string SDK_VERSION = "1.13.9";

#if UNITY_EDITOR
        private void OnValidate()
        {
            InAppIdType = _inAppIdType;
            SetDefines();
        }
#endif

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
            DontDestroyOnLoad(this);
#if UNITY_WEBGL && !UNITY_EDITOR
            Platform = EccentricJS.ECC_GetCurrentPlatform();
            Language = EccentricJS.ECC_GetLanguage();
            IsMobile = EccentricJS.ECC_IsMobile();
#else
            Platform = Platform.None;
            Language = EccentricJS.ECC_GetLanguage();
            IsMobile = EccentricJS.ECC_IsMobile();
#endif

#if GAMEDISTRIBUTION
            var gd = gameObject.AddComponent<GameDistribution>();
            gd.Init(_gameDistributionKey);
            CreateManagers();
#elif LAGGED
            var lagged = gameObject.AddComponent<LaggedAPIUnity>();
            lagged.DEV_ID = _developerId;
            lagged.PUBLISHER_ID = _publisherId;
            LaggedAPIUnity.Instance = lagged;
            if (Publisher == Publisher.Eccentric)
                lagged.InitWithoutAdsense();
            else
                lagged.Init();
            CreateManagers();
#elif JIO
            var jio = gameObject.AddComponent<JioWrapperJS>();
            jio.adSpotInterstitial = _adSpotInterstitial;
            jio.adSpotRewardedVideo = _adSpotRewardedVideo;
            jio.packageName = _packageName;
            jio.Init();
            CreateManagers();
#else
            _isTokenValid = CheckProtectedToken();
            if (!_isTokenValid)
            {
                var textMessage = Language switch
                {
                    Language.Russian => "Игра недоступна на этом сайте. Секретный ключ не был подтвержден!",
                    Language.Spanish => "El juego no está disponible en este sitio. ¡La clave secreta no ha sido verificada!",
                    Language.German => "Das Spiel ist auf dieser Seite nicht verfügbar. Der geheime Schlüssel wurde nicht verifiziert!",
                    Language.Turkish => "Oyun bu sitede mevcut değil. Gizli anahtar doğrulanmadı!",
                    
                    _ => "The game is not available on this site. The secret key has not been verified!"
                };
                EccentricJS.ECC_ShowCustomModal(textMessage, false);

                throw new Exception("!!!INVALID PROTECTED TOKEN!!!");
            }
            CreateManagers();
            StartCoroutine(AdManager.PauseOnStartCoroutine());
            PromoGame.Init();
            InAppPurchase.Init();
            
#endif


            _saveSystem.Init();
            _localisationDatas.AddRange(loadLocalisationDatas());
            LocalisationManager.CreateDictionary();
            _setQualityGraphic.SetQuality();
        }


        private void OnEnable()
        {
#if GAMEDISTRIBUTION
            GameDistribution.OnReadySDK += Subscribe;
#else
            Subscribe();
#endif
        }

        private IEnumerator Start()
        {
            Debug.LogWarning($"ECCENTRIC_SDK_{SDK_VERSION}");
            CreateTools(_isEnableTextSwitcher, _isEnableConsoleLog);
#if GAMEDISTRIBUTION
#elif LAGGED
            StartCoroutine(AdManager.CheckRewardAdCoroutine());
#elif JIO
#else
            AdManager.SwitchStickyBanner(AdManager.IsCanShowSticky());

            if (Platform == Platform.CRAZY_GAMES)
            {
                StartCoroutine(AdManager.UpdateTimerCoroutine());
            }
#endif

            yield return new WaitForSeconds(_timeBeforeInitialize);
           if (Platform == Platform.GAME_DISTRIBUTION)
           { 
               AdManager.ShowAd();
           }
            IsInitialized = true;
        }


        private void OnDisable()
        {
#if GAMEDISTRIBUTION
            GameDistribution.OnReadySDK -= Subscribe;
#else
            Unsubscribe();
#endif
        }

        private bool CheckProtectedToken()
        {

#if GAMEDISTRIBUTION
            return true;
#elif LAGGED
            return true;
#elif JIO
            return true;
#else
            if (!Application.isEditor && Application.platform != RuntimePlatform.WebGLPlayer)
                return true;

            if (string.IsNullOrEmpty(_token))
            {
                throw new Exception("ТОКЕН НЕ ЗАПОЛНЕН!");
            }

            return Application.isEditor || GP_Variables.GetString("TOKEN") == _token;
#endif
        }

        private void CreateManagers()
        {
            if (_saveSystemType == SaveSystemType.Prefs)
            {
                _saveSystem = new SaveSystemPrefs(_saveType);
                SaveSystemPrefs = _saveSystem as SaveSystemPrefs;
            }
            else
            {
                _saveSystem = new SaveSystemWithData(_saveType);
                SaveSystemWithData = _saveSystem as SaveSystemWithData;
            }

            CollectionPanelNew = new();
            PromoGame = new();
            App = new();
            InAppPurchase = new();
            LeaderboardManager = new();

            LocalisationManager = new(Language, _localisationDatas);
            _setQualityGraphic = new(IsMobile);

#if GAMEDISTRIBUTION
            AdManager = new AdManagerGamedistribution(Platform);
            AnalyticManager = new AnalyticManagerGamedistribution();
#elif LAGGED
            AdManager = new AdManagerLagged(Platform);
            AnalyticManager = new AnalyticManagerLagged();
#elif JIO
            AdManager = new AdManagerJio(Platform);
            AnalyticManager = new AnalyticManagerJio();
#else
            AdManager = new AdManagerOther(Platform);
            AnalyticManager = new AnalyticManagerOther();
#endif
        }

        private LocalisationDataSO[] loadLocalisationDatas() => Resources.LoadAll<LocalisationDataSO>("Localisation");

        private void CreateTools(bool isEnableSwitcherText, bool isEnableConsoleLog)
        {
            if (isEnableSwitcherText)
            {
                _textSwitcher = Resources.Load<TextSwitcher>("Tools/TextSwitcher");
                Instantiate(_textSwitcher);
            }

            if (isEnableConsoleLog)
            {
                _consoleViewer = Resources.Load<ConsoleViewer>("Tools/ConsoleViewer");
                Instantiate(_consoleViewer);
            }
        }

        private void Subscribe()
        {
            if (AdManager == null)
                return;

            AdManager.Subscribe();
            
#if GAMEDISTRIBUTION
#elif LAGGED
#elif JIO
#else
            InAppPurchase.Subscribe();
            CollectionPanelNew.Subscribe();
            LeaderboardManager.Subscribe();
#endif
        }

        private void Unsubscribe()
        {
            if (AdManager == null)
                return;

            AdManager.Unsubscribe();

#if GAMEDISTRIBUTION
#elif LAGGED
#elif JIO
#else
            InAppPurchase.Unsubscribe();
            CollectionPanelNew.Unsubscribe();
            LeaderboardManager.Unsubscribe();
#endif
        }
        
        public void SetYandexCurrency(string currencyName) //Вызывается через jslib
        {
            InAppPurchase.CurrencyOfPlatformYandex = currencyName;
        }
        private void SetDefines()
        {
#if UNITY_EDITOR
            var defines = BuildType switch
            {
                BuildType.GameDistribution => "GAMEDISTRIBUTION",
                BuildType.Lagged => "LAGGED",
                BuildType.Jio => "JIO",
                _ => "",
            };
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup, defines
            );
#endif
        }

    }

    public enum BuildType
    {
        GameDistribution,
        Lagged,
        Jio,
        Other,
    }

    public enum Publisher
    {
        Eccentric,
        Mamboo,
        Other,
    }

    public enum Language
    {
        Russian,
        English,
        German,
        Spanish,
        Turkish
    }

    public enum Platform
    {
        GAME_DISTRIBUTION,
        YANDEX,
        OK,
        VK,
        CRAZY_GAMES,
        LAGGED,
        KONGREGATE,
        VK_PLAY,
        JIO,
        None,
    }

    public enum InAppIdType
    {
        Id,
        Tag
    }

    public enum SaveSystemType
    {
        Data,
        Prefs,
    }

    public enum SaveType
    {
        LocalAndCloud,
        OnlyLocal,
    }
}
