using System;
using UnityEngine;

public class CatSpiner : MonoBehaviour
{
    private CatJumper _jumper;
    private Block _currentBlock;
    private Block _nextBlock;
    private CatAnimation _animator;

    private void Awake()
    {
        _jumper = GetComponent<CatJumper>();
        _animator = GetComponent<CatAnimation>();
    }

    public void Spine(Action callback, Block spinerBlock, Block nextBlock)
    {
        if (spinerBlock is BlockSpiner spiner)
        {
            _currentBlock = spinerBlock;
            _nextBlock = nextBlock;

            _animator.SetRotate();

            spiner.StartSpine(Jump, callback);
        }
    }

    private void Jump(Action callback)
    {
        _jumper.Jump(callback, _currentBlock, _nextBlock);
    }
}
