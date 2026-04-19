using UnityEngine;
using DG.Tweening;
using System.Collections;

public class WorldObject : MonoBehaviour
{
    [SerializeField] private bool _isMovingInGame = false;
    [SerializeField] private Direction _startDirection;
    [SerializeField] private float _speedDecreasser = 2;
    [SerializeField] private AnimationCurve _animationCurve;
    [Space]

    private float _yPositionAtStart = 3f;
    private float _yPositionAtShowing;
    private float _xPositionAtShowing;

    private Vector3 _scale;
    private Coroutine _movingLeftAndRight;
    private float _movingBorder = 0.75f;

    public void Hide()
    {
        _scale = transform.localScale;

        _yPositionAtShowing = transform.localPosition.y;
        _xPositionAtShowing = transform.localPosition.x;

        transform.localScale = Vector3.zero;
        Vector3 startPosition = transform.localPosition;
        startPosition.y = _yPositionAtStart;
        startPosition.x = 0;
        transform.localPosition = startPosition;
    }

    public virtual void Show()
    {
        transform.DOLocalMoveY(_yPositionAtShowing, 1f);
        transform.DOLocalMoveX(_xPositionAtShowing, 1f);
        transform.DOScale(_scale, 1f).OnComplete(() =>
        {
            if (_isMovingInGame)
                _movingLeftAndRight = StartCoroutine(MovingLeftAndRight());
        });
    }

    public void StartMove()
    {
        if (_isMovingInGame)
            _movingLeftAndRight = StartCoroutine(MovingLeftAndRight());

    }

    protected void StopMove()
    {
        if (_movingLeftAndRight != null)
        {
            _isMovingInGame = false;
            StopCoroutine(_movingLeftAndRight);
            _movingLeftAndRight = null;
        }
    }

    private IEnumerator MovingLeftAndRight()
    {
        float xDirection;
        float speed = SongManager.Speed;
        float distance;
        float time;
        Tween tween;

        switch (_startDirection)
        {
            case Direction.Left:
                xDirection = -_movingBorder;
                break;
            case Direction.Right:
                xDirection = _movingBorder;
                break;
            default:
                xDirection = _movingBorder;
                break;
        }

        while (_isMovingInGame)
        {
            distance = this.transform.position.x - xDirection;

            time = Mathf.Abs(distance) / speed * _speedDecreasser;

            tween = this.transform.DOMoveX(xDirection, time).SetEase(_animationCurve);

            yield return tween.WaitForCompletion();

            xDirection *= -1;

            yield return null;
        }

        _movingLeftAndRight = null;
    }

    public enum Direction
    {
        Left,
        Right
    }
}
