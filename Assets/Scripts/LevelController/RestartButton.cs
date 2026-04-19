using Eccentric;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    [SerializeField] private RestartButtonUI _button;

    private Button _restartButton;

    private void Awake()
    {
        _restartButton = _button.GetComponent<Button>();
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        GameProgressHolder.Instance.SendLostEvent();
        LevelLoader.Instance.RestartLevel();
    }
}
