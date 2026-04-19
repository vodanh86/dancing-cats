using Eccentric;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;

    private Button _rewardButton;
    private Coroutine _waitingDelay;

    private void Awake()
    {
        _rewardButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _rewardButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _rewardButton.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        TryWatchAD();
    }

    private void TryWatchAD()
    {
        if (_waitingDelay == null)
        {
            _waitingDelay = StartCoroutine(WaitingDelay());
            EccentricInit.Instance.AdManager.ShowRewardAd(GetReward);
        }
    }

    private void GetReward()
    {
        _wallet.Add(200);
        EccentricInit.Instance.SaveSystemWithData.Save(SaveSystemWithData.PlayerData);
    }

    private IEnumerator WaitingDelay()
    {
        yield return new WaitForSeconds(2f);

        _waitingDelay = null;
    }
}
