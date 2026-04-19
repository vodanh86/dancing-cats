using Eccentric;
using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private int _moneyEarnedPerLevel = 0;
    public int MoneyEarnedPerLevel => _moneyEarnedPerLevel;

    public Action<int> AmountChanged;


    public void Add(int addedAmount)
    {
        if (addedAmount > 0)
        {
            _moneyEarnedPerLevel += addedAmount;

            SaveSystemWithData.PlayerData.CurrencyAmount += addedAmount;

            AmountChanged?.Invoke(SaveSystemWithData.PlayerData.CurrencyAmount);
        }
    }

    public bool IsAbleToSpend(int demandedAmount)
    {
        return demandedAmount <= SaveSystemWithData.PlayerData.CurrencyAmount;
    }

    public void Spend(int spendedAmount)
    {
        if (spendedAmount > 0 && spendedAmount <= SaveSystemWithData.PlayerData.CurrencyAmount)
        {

            SaveSystemWithData.PlayerData.CurrencyAmount -= spendedAmount;

            AmountChanged?.Invoke(SaveSystemWithData.PlayerData.CurrencyAmount);
        }
    }
}