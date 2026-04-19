using Eccentric;
using UnityEngine;
using UnityEngine.UI;

public class LocalisationImageSwitcher : MonoBehaviour
{
    [SerializeField] private Sprite _spriteRu;
    [SerializeField] private Sprite _spriteEn;
    [SerializeField] private Sprite _spriteGe;
    [SerializeField] private Sprite _spriteTr;
    [SerializeField] private Sprite _spriteSp;
    [SerializeField] private Image _image;

    private void Start()
    {
        if (EccentricInit.Instance.Language == Eccentric.Language.Russian)
            _image.overrideSprite = _spriteRu;
        else if (EccentricInit.Instance.Language == Eccentric.Language.German)
            _image.overrideSprite = _spriteGe;
        else if (EccentricInit.Instance.Language == Eccentric.Language.Turkish)
            _image.overrideSprite = _spriteTr;
        else if (EccentricInit.Instance.Language == Eccentric.Language.Spanish)
            _image.overrideSprite = _spriteSp;
        else
            _image.overrideSprite = _spriteEn;
    }
}
