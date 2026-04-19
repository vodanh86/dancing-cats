using UnityEngine;
using UnityEngine.UI;

namespace Eccentric
{
    [RequireComponent(typeof(Button))]
    public class LeaderboardButton : MonoBehaviour
    {
        private Button _btn;

        void Awake()
        {
            _btn = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _btn.onClick.AddListener(Show);
        }

        private void OnDisable()
        {
            _btn.onClick.RemoveAllListeners();
        }

        private void Show()
        {
            EccentricInit.Instance.LeaderboardManager.ShowLeaderboard();
        }
    }
}