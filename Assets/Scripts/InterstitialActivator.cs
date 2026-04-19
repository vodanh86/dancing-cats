using UnityEngine;

public class InterstitialActivator : MonoBehaviour
{
    public void TryWatchAds()
    {
        if (InterstitialManager.Instance != null)
            InterstitialManager.Instance.TryShowAD();
    }
}
