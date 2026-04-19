using UnityEngine;
using UnityEngine.UI;

namespace Eccentric
{
    public class EccentricUIController : MonoBehaviour
    {
        [SerializeField] private Button _collectionButton;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Button _socialButton;
        [SerializeField] private Button _loginButton;

        private void Start()
        {
            switch (EccentricInit.Instance.Platform)
            {
                case Platform.YANDEX:
                    _collectionButton.gameObject.SetActive(true);
                    _leaderboardButton.gameObject.SetActive(true);
                    _socialButton.gameObject.SetActive(false);
                    _loginButton.gameObject.SetActive(false);
                    break;
                case Platform.OK or Platform.VK:
                    _collectionButton.gameObject.SetActive(false);
                    _leaderboardButton.gameObject.SetActive(true);
                    _socialButton.gameObject.SetActive(true);
                    _loginButton.gameObject.SetActive(false);
                    break;
                case Platform.CRAZY_GAMES:
                    _collectionButton.gameObject.SetActive(false);
                    _leaderboardButton.gameObject.SetActive(false);
                    _socialButton.gameObject.SetActive(false);
                    _loginButton.gameObject.SetActive(false);
                    break;
                default:
                    _collectionButton.gameObject.SetActive(false);
                    _leaderboardButton.gameObject.SetActive(false);
                    _socialButton.gameObject.SetActive(false);
                    _loginButton.gameObject.SetActive(false);
                    break;
            }

            if (EccentricInit.Instance.SaveType == SaveType.OnlyLocal)
                _loginButton.gameObject.SetActive(false);

            switch (EccentricInit.Instance.Language)
            {
                case Language.German or Language.Spanish or Language.Turkish:
                    _collectionButton.gameObject.SetActive(false);
                    _socialButton.gameObject.SetActive(false);
                    
                    break;
            }
        }
    }
}