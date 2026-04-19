using System;
using UnityEngine;

public class CatGlider : MonoBehaviour
{
    private CatJumper _jumper;
    private Block _currentBlock;
    private Block _nextBlock;
    private CatAnimation _animator;

    public Action GlideDone;

    private void Awake()
    {
        _jumper = GetComponent<CatJumper>();
        _animator = GetComponent<CatAnimation>();
    }

    public void Glide(Action callback, Block gliderBlock, Block nextBlock)
    {
        if (gliderBlock is BlockGlider glider)
        {
            GlideDone = callback;
            _currentBlock = gliderBlock;
            _nextBlock = nextBlock;

            _animator.SetRotate();
            glider.StartGlide(Jump, this.transform);
        }
    }

    private void Jump()
    {
        _jumper.Jump(SendDoneAction, _currentBlock, _nextBlock);
    }

    private void SendDoneAction()
    {
        GlideDone?.Invoke();
    }
}
