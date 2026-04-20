using UnityEngine;
using System.Runtime.InteropServices;

namespace Eccentric
{
    public class EccentricJS : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void ECC_ReloadPageExtern();

        [DllImport("__Internal")]
        private static extern bool ECC_IsMobileExtern();

        [DllImport("__Internal")]
        private static extern string ECC_LanguageExtern();

        [DllImport("__Internal")]
        private static extern string ECC_GetCurrentPlatformExtern();

        [DllImport("__Internal")]
        private static extern string ECC_ShowPromoGameModalExtern(int idGp, int idYa, string title);

        [DllImport("__Internal")]
        private static extern void ECC_ShowCustomModalExtern(string text, bool isCanClose);

        [DllImport("__Internal")]
        private static extern void ECC_ShowCollectionModalExtern(string nameCollection);

        [DllImport("__Internal")]
        private static extern void ECC_SetCollectionDataExtern(int index, string nameGame, string link,
            string urlBanner);

        [DllImport("__Internal")]
        private static extern void ECC_ShowLeaderboardExtern();

        [DllImport("__Internal")]
        private static extern void ECC_SetLeaderboardDataExtern(int index, int number, string avatarUrl, string name,
            int score, bool isPlayer);

        [DllImport("__Internal")]
        private static extern void ECC_ShowRequestReviewExtern();

        [DllImport("__Internal")]
        private static extern void ECC_ShowLoginPanelExtern();

        [DllImport("__Internal")]
        private static extern bool ECC_IsIOSExtern();
        
        [DllImport("__Internal")]
        private static extern void ECC_GetCurrencyIconYandexExtern();

        public static Language ECC_GetLanguage()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var lang = ECC_LanguageExtern();
            return lang switch
            {
                "ru-RU" or "ru" or "ru-ru" => Language.Russian,
                "tr" or "tr-TR" => Language.Turkish,
                "es" or "es-ES" => Language.Spanish,
                "de" or "de-DE" => Language.German,
                _ => Language.English,
            };
#else
            return Application.systemLanguage switch
            {
                SystemLanguage.Russian => Language.Russian,
                SystemLanguage.Turkish => Language.Turkish,
                SystemLanguage.Spanish => Language.Spanish,
                SystemLanguage.German => Language.German,
                _ => Language.English,
            };
#endif
        }

        public static bool ECC_IsMobile()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return ECC_IsMobileExtern();
#else
            return Application.isMobilePlatform;
#endif
        }


        public static void ECC_ReloadPage()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ECC_ReloadPageExtern();
#else
            Debug.LogWarning("ECC_ReloadPageExtern");
#endif
        }

        public static Platform ECC_GetCurrentPlatform()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var platform = ECC_GetCurrentPlatformExtern();
            return platform.Contains("yandex") ? Platform.YANDEX
                : platform.Contains("gamedistribution") ? Platform.GAME_DISTRIBUTION
                : platform.Contains("crazygames") ? Platform.CRAZY_GAMES
                : platform.Contains("ok.ru") || platform.Contains("ok.com") ? Platform.OK
                : platform.Contains("vk.ru") || platform.Contains("vk.com") ? Platform.VK
                : platform.Contains("lagged") ? Platform.LAGGED
                : platform.Contains("kongregate") ? Platform.KONGREGATE
                : platform.Contains("vkplay") ? Platform.VK_PLAY
                : Platform.None;
#else
            return Platform.None;
#endif
        }

        public static void ECC_ShowPromoModal(int idGp, int idYa, string title)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ECC_ShowPromoGameModalExtern(idGp, idYa, title);
#else
            Debug.LogWarning("ECC_ShowPromoGameModalExtern");
#endif
        }

        public static void ECC_ShowCustomModal(string text, bool isCanClose = true)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ECC_ShowCustomModalExtern(text, isCanClose);
#else
            Debug.LogWarning("ECC_ShowCustomModalExtern");
#endif
        }


        public static void ECC_ShowCollectionModal(string nameCollection)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ECC_ShowCollectionModalExtern(nameCollection);
#else
            Debug.LogWarning("ECC_ShowCollectionModalExtern");
#endif
        }

        public static void ECC_SetCollectionData(int index, string nameGame, string link,
            string urlBanner)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ECC_SetCollectionDataExtern(index, nameGame, link, urlBanner);
#else
            Debug.LogWarning("ECC_SetCollectionDataExtern");
#endif
        }

        public static void ECC_ShowLeaderboard()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ECC_ShowLeaderboardExtern();
#else
            Debug.LogWarning("ECC_ShowLeaderboardExtern");
#endif
        }

        public static void ECC_SetLeaderboardData(int index, int number, string avatarUrl, string name, int score,
            bool isPlayer)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ECC_SetLeaderboardDataExtern(index, number, avatarUrl, name, score, isPlayer);
#else
            Debug.LogWarning("ECC_SetLeaderboardDataExtern");
#endif
        }

        public static void ECC_ShowRequestReview()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ECC_ShowRequestReviewExtern();
#else
            Debug.LogWarning("ECC_ShowRequestReviewExtern");
#endif
        }

        public static void ECC_ShowLoginPanel()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ECC_ShowLoginPanelExtern();
#else
            Debug.LogWarning("ECC_ShowLoginPanelExtern");
#endif
        }

        public static bool ECC_IsIOS()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
           return ECC_IsIOSExtern();
#else
            return Application.platform == RuntimePlatform.IPhonePlayer;
#endif
        }
        public static void ECC_GetCurrencyIconYandex()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ECC_GetCurrencyIconYandexExtern();
#else
            Debug.LogWarning("ECC_GetCurrencyIconYandexExtern");
           
#endif
        }
    }
}
