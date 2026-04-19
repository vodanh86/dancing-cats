using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
using GamePush;
#endif

namespace Eccentric
{
    public class CollectionsButton : MonoBehaviour
    {
        [SerializeField] private Texture _defautTextureRU;
        [SerializeField] private Texture _defautTextureEN;
        [SerializeField] private Texture _textureMamboo;
        [SerializeField] private Button _button;
        [SerializeField] private RawImage _image;
        private static Texture _texture;
        private static bool _isDownloaded;
        private const string URL_TEXTURE = "URL_BANNER";

#if !GAMEDISTRIBUTION && !LAGGED && !JIO
        void Start()
        {

            string urlIcon = null;

            if (GP_Variables.Has(URL_TEXTURE))
            {
                urlIcon = GP_Variables.GetString(URL_TEXTURE);
            }
            else
            {
                if (EccentricInit.Instance.Publisher == Publisher.Mamboo)
                {
                    _image.texture = _textureMamboo;
                }
                else
                {
                    _image.texture = EccentricInit.Instance.Language == Language.Russian
                        ? _defautTextureRU
                        : _defautTextureEN;
                }

                return;
            }

            if (!string.IsNullOrEmpty(urlIcon))
            {
                if (!_isDownloaded)
                    StartCoroutine(DownloadIcon(urlIcon));
                else
                    _image.texture = _texture;
            }

        }


        IEnumerator DownloadIcon(string url)
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);

            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                _texture = DownloadHandlerTexture.GetContent(webRequest);
                _image.texture = _texture;
                _isDownloaded = true;
            }
            else
            {
                Debug.LogError($"Error download image from {url}");
            }
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(EccentricInit.Instance.CollectionPanelNew.Show);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
            StopAllCoroutines();
        }

#endif
    }
}