using DG.Tweening;
using Eccentric;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    [SerializeField] private CanvasGroupView _groupView;
    [SerializeField] private Button _interactivityButton;

    private void Start()
    {
        StartCoroutine(Waiting());
    }

    private IEnumerator Waiting()
    {
        yield return new WaitUntil(() => EccentricInit.IsInitialized);

        Shop.IsFetched = false;
        EccentricInit.Instance.InAppPurchase.FetchPurchases();
        GamePush.GP_Analytics.Goal("GAME_LOADED", 0);

        if (!EccentricInit.Instance.IsMobile)
            _groupView.gameObject.SetActive(false);
        else
            _interactivityButton.transform.DOScale(1.1f, 0.75f).SetLoops(-1, LoopType.Yoyo);

        LevelLoader.Instance.LoadLevel();
    }
}
