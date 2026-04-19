using Eccentric;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSetter : MonoBehaviour
{
    [SerializeField] private Image _currentImage;
    [SerializeField] private List<Sprite> _sprites = new List<Sprite>();

    private Platform _currentPlatform;

    private void Start()
    {
        GetPlatform();

        SetSprite();
    }

    private void GetPlatform()
    {
        _currentPlatform = EccentricInit.Instance.Platform;
    }

    private void SetSprite()
    {
        _currentImage.sprite = _currentPlatform switch
        {
            Platform.YANDEX => _sprites[0],
            Platform.OK => _sprites[1],
            Platform.VK => _sprites[2],
            _ => _sprites[3],
        };
    }
}
