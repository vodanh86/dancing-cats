using UnityEngine;
using UnityEngine.UI;

namespace Eccentric
{
    [RequireComponent(typeof(Image))]
    public class LocalisationImage : MonoBehaviour
    {
        [SerializeField] private Sprite _ruSprite;
        [SerializeField] private Sprite _enSprite;

        private Image _image;

        private void Awake() => _image = GetComponent<Image>();

        private void Start() =>
            _image.sprite = EccentricInit.Instance.Language == Language.Russian ? _ruSprite : _enSprite;
    }
}