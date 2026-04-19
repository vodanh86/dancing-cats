using UnityEngine;
using UnityEngine.UI;
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
using GamePush;
#endif

namespace Eccentric
{
    public class LoginSystem : MonoBehaviour
    {
        private Button _loginButton;

        private void Awake()
        {
            _loginButton = GetComponent<Button>();
        }

        private void Start()
        {
            ToggleLoginButton(false);
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
        if (GP_Player.IsLoggedIn()) return;
        ToggleLoginButton(true);
#endif
        }
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
    private void OnEnable()
    {
        GP_Player.OnLoginComplete += OnLoginCompleteHandler;
        _loginButton.onClick.AddListener(Login);
    }

    private void OnLoginCompleteHandler()
    {
        ToggleLoginButton(false);
        EccentricJS.ECC_ReloadPage();
    }

    private void OnDisable()
    {
        GP_Player.OnLoginComplete -= OnLoginCompleteHandler;
        _loginButton.onClick.RemoveAllListeners();
    }

  private void Login()
    {
        GP_Player.Login();
    }


#endif
        private void ToggleLoginButton(bool enable)
        {
            _loginButton.gameObject.SetActive(enable);
        }
    }
}