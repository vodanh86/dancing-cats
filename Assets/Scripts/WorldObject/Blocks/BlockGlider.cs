using System;
using UnityEngine;
using DG.Tweening;

public class BlockGlider : Block
{
    private BlockGliderEnd _end;
    private GamePointsSphere[] _sphereList;

    private Coroutine _gliding;

    public Action GlidingEnded;

    protected override void Awake()
    {
        base.Awake();

        _end = GetComponentInChildren<BlockGliderEnd>();
        _sphereList = GetComponentsInChildren<GamePointsSphere>();

        foreach (GamePointsSphere sphere in _sphereList)
            sphere.gameObject.transform.SetParent(this.gameObject.transform.parent);

        _end.gameObject.transform.SetParent(this.gameObject.transform.parent);

        var endPosition = _end.transform.position;
        endPosition.x = 0;
        endPosition.y = 0.74f;
        _end.transform.position = endPosition;
    }

    public void StartGlide(Action callback, Transform newParent)
    {
        if (_gliding == null)
        {
            GlidingEnded = callback;

            this.transform.SetParent(newParent);
            this.transform.DOLocalMove(Vector3.zero, 0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _end.gameObject)
        {
            this.transform.DOKill();
            this.transform.SetParent(null);
            GlidingEnded?.Invoke();
        }
    }

    protected override BlockType InitType() => BlockType.Glide;
}
