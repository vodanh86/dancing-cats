using UnityEngine;
using UnityEngine.UI;
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
using GamePush;
#endif

namespace Eccentric
{
    [RequireComponent(typeof(Button))]
    public class SocialButton : MonoBehaviour
    {
        private Button _button;
        private void Awake() => _button = GetComponent<Button>();

#if !GAMEDISTRIBUTION && !LAGGED && !JIO
        private void OnEnable() => _button.onClick.AddListener(() => GP_Socials.Share());
        private void OnDisable() => _button.onClick.RemoveAllListeners();
#endif
    }
}