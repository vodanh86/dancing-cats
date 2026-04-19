using UnityEngine;

namespace Eccentric
{
    [RequireComponent(typeof(MeshRenderer))]
    public class LocalisationTexture : MonoBehaviour
    {
        [SerializeField] private Texture _ruTexture;
        [SerializeField] private Texture _enTexture;
        private MeshRenderer _meshRenderer;
        private void Awake() => _meshRenderer = GetComponent<MeshRenderer>();

        private void Start() =>
            _meshRenderer.material.mainTexture =
                EccentricInit.Instance.Language == Language.Russian ? _ruTexture : _enTexture;
    }
}