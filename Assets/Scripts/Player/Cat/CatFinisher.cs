using System;
using UnityEngine;

public class CatFinisher : MonoBehaviour
{
    private CatAnimation _animator;

    public event Action Finished;

    private void Awake()
    {
        _animator = GetComponent<CatAnimation>();
    }

    public void Finish()
    {
        _animator.SetIdle();
        Finished?.Invoke();
    }
}
