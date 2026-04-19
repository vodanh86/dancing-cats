using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CatWings : MonoBehaviour
{
    [SerializeField] private float _wingsTime = 3f;
    [SerializeField] private Transform _wings;

    private bool _isFlying;
    private CatAnimation _animator;

    private Coroutine _flying;

    public bool IsFlying => _isFlying;

    private void Awake()
    {
        _wings.localScale = Vector3.zero;
        _animator = GetComponent<CatAnimation>();
    }

    public void StartToFly()
    {
        if (_flying == null)
        {
            _animator.SetIdle();
            _isFlying = true;
            _flying = StartCoroutine(Flying());
        }
    }

    private IEnumerator Flying()
    {
        Tween tween;

        tween = this.transform.DOMoveY(-1f, 0.5f);
        yield return tween.WaitForCompletion();

        _wings.DOScale(1f, 0.7f);
        tween = this.transform.DOMoveY(0.6f, 0.7f);

        yield return tween.WaitForCompletion();

        tween = this.transform.DOMoveY(0, 0.5f);

        yield return tween.WaitForCompletion();

        var delay = new WaitForSeconds(_wingsTime - 1.2f);

        yield return delay;

        _wings.DOScale(0, 0.7f);
        _flying = null;
        _isFlying = false;
    }
}
