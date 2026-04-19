using UnityEngine;

public class CatSkinsController : MonoBehaviour
{
    [SerializeField] private CatSkin[] _skins;

    public CatSkin[] Skins => _skins;

    private void Awake()
    {
        _skins = GetComponentsInChildren<CatSkin>(true);
    }

    public void ActivateSkin(string skinID)
    {
        foreach (CatSkin skin in _skins)
        {
            if (skin.gameObject.name == skinID)
                skin.gameObject.SetActive(true);
            else
                skin.gameObject.SetActive(false);
        }
    }
}
