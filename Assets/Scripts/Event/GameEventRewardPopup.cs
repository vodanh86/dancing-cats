using DG.Tweening;
using UnityEngine;

public class GameEventRewardPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroupView _view;
    [SerializeField] private Transform _targetTranform;

    public void Open()
    {
        _targetTranform.DOKill();
        _targetTranform.DOScale(Vector3.one, .5f);
        _view.SetVisibilityFast(true);
    }

    public void Close()
    {
        _targetTranform.DOKill();
        _targetTranform.DOScale(Vector3.zero, .5f);
        _view.SetVisibility(false);
    }
}
