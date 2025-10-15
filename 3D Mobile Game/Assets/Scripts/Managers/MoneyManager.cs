using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

public class MoneyManager : MonoBehaviour
{

    public static MoneyManager Instance { get; private set; }

    public event Action<int> onMoneyChanged;

    [Header("Money Settings")]

    public int startMoney = 100;
    public int currentMoney;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentMoney = startMoney;
        onMoneyChanged?.Invoke(currentMoney);
    }

    public int GetMoney()
    {
        return currentMoney;
    }

    public bool HasEnough(int amount)
    {
        return currentMoney >= amount;
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        onMoneyChanged?.Invoke(currentMoney);
        Debug.Log($"+{amount} money added, total: {currentMoney}");
    }

    public bool RemoveMoney(int amount)
    {
        if (!HasEnough(amount))
            return false;

        currentMoney -= amount;
        onMoneyChanged?.Invoke(currentMoney);
        Debug.Log($"Money spent: -{amount}, Total: {currentMoney}");
        return true;
    }
}
