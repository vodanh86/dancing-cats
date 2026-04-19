using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CatJumper : MonoBehaviour
{
    [SerializeField] private AnimationCurve _jumpHieghtCurve;
    [SerializeField] private AnimationCurve _jumpAnimationCurve;

    private CatAnimation _animator;
    private Coroutine _playingAnimation;
    private float _speed;

    private void Awake()
    {
        _animator = GetComponent<CatAnimation>();
    }
    private void Start()
    {
        _speed = SongManager.Speed;
    }

    public void Jump(Action callback, Block currentBlock, Block nextBlock)
    {
        if (_playingAnimation == null)
            _playingAnimation = StartCoroutine(PlayingAnimation(callback, nextBlock.transform.position.z));

        currentBlock.Fall();
    }

    private void TriggerAnimator(float jumpDistance)
    {
        if (jumpDistance >= 2f)
            _animator.SetJump(true);
        else
            _animator.SetJump(false);
    }

    private IEnumerator PlayingAnimation(Action callback, float jumpDistance)
    {

        var distance = jumpDistance - transform.position.z;
        var timeToReach = distance / _speed;
        TriggerAnimator(distance);

        var jumpPower = _jumpHieghtCurve.Evaluate(distance);

        Tween tween = transform.DOLocalJump(Vector3.zero, jumpPower, 1, timeToReach).SetEase(_jumpAnimationCurve);

        yield return tween.WaitForCompletion();

        _playingAnimation = null;

        callback?.Invoke();
    }
}
