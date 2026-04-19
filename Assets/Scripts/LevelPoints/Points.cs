using System;
using UnityEngine;

public class Points : MonoBehaviour
{
    private int _amount = 0;
    public int Amount => _amount;

    public Action<int> AmountChanged;

    public void Add(int addedAmount)
    {
        if (addedAmount > 0)
        {
            _amount += addedAmount;
            AmountChanged?.Invoke(_amount);
        }
    }
}