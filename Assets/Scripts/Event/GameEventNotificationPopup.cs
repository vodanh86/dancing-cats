using System.Collections;
using DG.Tweening;
using Eccentric;
using TMPro;
using UnityEngine;

public class GameEventNotificationPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroupView _view;
    [SerializeField] private TMP_Text _timeLeftText;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Color32 _textColor;

    public void Close()
    {
        _targetTransform.DOScale(Vector3.zero, 0.5f);
        _view.SetVisibility(false);
    }

    public void Open()
    {
        StartCoroutine(InitializeTimerText());
        _view.SetVisibilityFast(true);
        _targetTransform.localScale = Vector3.one * 0.5f;
        _targetTransform.DOScale(Vector3.one, 0.3f);
    }

    private IEnumerator InitializeTimerText()
    {
        string targetKey = "";
        int timeLeft = GameEvent.Instance.GameEventData.TimeLeftMinutes;

        if (timeLeft % 10 == 1)
        {
            targetKey = "minute1";

            if (EccentricInit.Instance.Language != Eccentric.Language.Russian && timeLeft != 1)
            {
                targetKey = "minute0";
            }
        }
        else if (timeLeft % 10 >= 2 && timeLeft % 10 <= 4)
        {
            targetKey = "minute2";
        }
        else
        {
            targetKey = "minute0";
        }


        _timeLeftText.text =
            EccentricInit.Instance.LocalisationManager.GetText("TimeLeft1") + timeLeft + " " +
            EccentricInit.Instance.LocalisationManager.GetText(targetKey) + "" +
            EccentricInit.Instance.LocalisationManager.GetText("TimeLeft2");


        if (GameEvent.Instance.GameEventData.GotSkin)
        {
            _timeLeftText.text = EccentricInit.Instance.LocalisationManager.GetText("Already Got");
            yield break;
        }

        yield return null;

        TextColorChanger.SetColor(_timeLeftText, 2, _textColor);
        TextColorChanger.SetColor(_timeLeftText, 3, _textColor);
    }
}
