using UnityEngine;

public class CatAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private const string Idle = nameof(Idle);
    private const string Jump = nameof(Jump);
    private const string LongJump = nameof(LongJump);
    private const string Rotate = nameof(Rotate);

    private bool _isRotating = false;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void SetJump(bool isLongJump)
    {
        _isRotating = false;

        if (isLongJump)
            _animator.SetTrigger(LongJump);
        else
            _animator.SetTrigger(Jump);
    }

    public void SetRotate()
    {
        if (!_isRotating)
            _animator.SetTrigger(Rotate);

        _isRotating = true;
    }

    public void SetIdle()
    {
        _isRotating = false;
        _animator.SetTrigger(Idle);
    }
}
