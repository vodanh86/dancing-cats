using System;
using System.Collections;
using UnityEngine;

public class BlockSpiner : Block
{
    [Space]
    [Header("Size Settings")]
    [SerializeField][Range(2, 10)] private int _blockLength = 2;

    private int _blockTriggering = 1;
    private float _speed;

    private Coroutine _spinning;

    public int BlockLength => _blockLength;

    private void Start()
    {
        _speed = SongManager.Speed;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        BlockSpinerBuilder blockSpinerBuilder = GetComponent<BlockSpinerBuilder>();
        if (blockSpinerBuilder)
        {
            blockSpinerBuilder.UpdateSize();
        }
    }
#endif
    public void StartSpine(Action<Action> callback, Action alternativeCallback)
    {
        if (_spinning == null)
        {
            if (_blockTriggering >= _blockLength)
            {
                callback?.Invoke(alternativeCallback);
            }
            else
            {
                _spinning = StartCoroutine(Gliding(alternativeCallback));
            }
        }
    }

    private IEnumerator Gliding(Action alternativeCallback)
    {
        var delay = new WaitForSeconds((1f / _speed) - Time.deltaTime);

        yield return delay;

        _blockTriggering++;
        _spinning = null;

        alternativeCallback?.Invoke();
    }

    protected override BlockType InitType() => BlockType.Spin;
}

