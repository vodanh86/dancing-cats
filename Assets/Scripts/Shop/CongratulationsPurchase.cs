using UnityEngine;
using UnityEngine.UI;

public class CongratulationsPurchase : MonoBehaviour
{
    [SerializeField] private GameObject _background;
    [SerializeField] private Image _cat;

    public static CongratulationsPurchase Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void Activate(Sprite sprite)
    {
        _background.SetActive(true);
        _cat.overrideSprite = sprite;
    }
}
