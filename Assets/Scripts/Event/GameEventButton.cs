using UnityEngine;
using UnityEngine.UI;

public class GameEventButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void Start()
    {
        if (GameEvent.Instance == null)
            _button.gameObject.SetActive(false);
    }

    private void OnButtonClick()
    {
        GameEvent.Instance.OpenMenu();
    }
}
