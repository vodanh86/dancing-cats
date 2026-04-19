using UnityEngine;
using DG.Tweening;

public class CanvasGroupView : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private float _fadeDuration = 0.5f;
    private const float _hideAlfaValue = 0;
    private const float _showAlfaValue = 1;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetVisibilityFast(bool isVisible)
    {
        _canvasGroup.alpha = isVisible ? _showAlfaValue : _hideAlfaValue;
        _canvasGroup.interactable = isVisible;
        _canvasGroup.blocksRaycasts = isVisible;
    }

    public void SetVisibility(bool isVisible)
    {
        if (isVisible)
        {
            _canvasGroup.interactable = isVisible;
            _canvasGroup.blocksRaycasts = isVisible;
        }

        var alfaValue = isVisible ? _showAlfaValue : _hideAlfaValue;

        _canvasGroup.DOFade(alfaValue, _fadeDuration).OnComplete(() =>
        {
            if (!isVisible)
            {
                _canvasGroup.interactable = isVisible;
                _canvasGroup.blocksRaycasts = isVisible;
            }
        });
    }
}
