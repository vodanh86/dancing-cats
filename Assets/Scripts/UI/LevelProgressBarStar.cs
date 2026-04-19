using AssetKits.ParticleImage;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressBarStar : MonoBehaviour
{
    [SerializeField] private Image _star;

    private bool _isActivated = false;
    private Animator _animator;
    private ParticleImage _particleImage;

    public bool IsActivated => _isActivated;

    private void Awake()
    {
        _star = GetComponentInChildren<Image>();
        _star.gameObject.SetActive(false);
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        _particleImage = GetComponentInChildren<ParticleImage>();
    }

    public void Activate()
    {
        _isActivated = true;
        _star.gameObject.SetActive(true);
        _animator.enabled = true;
        _particleImage.Play();
    }
}
