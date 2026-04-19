using Eccentric;
using System.Collections;
using UnityEngine;

public class InterstitialManager : MonoBehaviour
{
    public static InterstitialManager Instance { get; private set; }

    private float _timeBetweenADS = 59f;
    private bool _isADSAvailable = true;
    private Coroutine _waitingDelay;

    private void Awake()
    {
        Instance = this;
    }

    public void TryShowAD()
    {
        if (_isADSAvailable)
        {
            _isADSAvailable = false;
            _waitingDelay = StartCoroutine(WaitingDelay());

            EccentricInit.Instance.AdManager.ShowAd();
        }
    }

    private IEnumerator WaitingDelay()
    {
        yield return new WaitForSeconds(_timeBetweenADS);

        _isADSAvailable = true;
        _waitingDelay = null;
    }
}
